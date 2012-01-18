namespace odm.ui.activities
    open System
    open System.Collections.Generic
    open System.Collections.ObjectModel
    open System.IO
    open System.Linq
    open System.Net
    open System.Threading
    open System.Windows
    open System.Windows.Threading
    
    open Microsoft.Practices.Unity
    //open Microsoft.Practices.Prism.Commands
    //open Microsoft.Practices.Prism.Events

    //open Org.BouncyCastle.X509
    open System.Security.Cryptography.X509Certificates
    open odm.ui

    open onvif.services
    open onvif.utils

    open odm.onvif
    open odm.core
    open odm.infra
    open utils
    //open odm.models
    open utils.fsharp

    type CertificateManagementActivity(ctx:IUnityContainer) = class
        let session = ctx.Resolve<INvtSession>()
        let dev = session :> IDeviceAsync
        
        let show_error(err:Exception) = async{
            dbg.Error(err)
            do! ErrorView.Show(ctx, err) |> Async.Ignore
        }
        
        let load() = async{
            let! certificates, statuses = Async.Parallel(
                dev.GetCertificates(),
                dev.GetCertificatesStatus()
            )
//            for cert in certificates do
//                let x509 = new X509Certificate2(cert.Certificate1.Data)
//                let subj = x509.Subject
//                let subjName = x509.SubjectName
//                //let certParser = new X509CertificateParser()
//                //let x509 = certParser.ReadCertificate(cert.Certificate1.Data)
//                x509.ToString() |> ignore

            let model = new CertificatesView.Model(
                certificates = certificates,
                enabled = (
                    statuses |>Seq.filter (fun cert-> cert.Status ) |> Seq.map (fun cert->cert.CertificateID )|> Seq.toArray
                )
            )
            return model
        }
        
        ///<summary></summary>
        member private this.Main() = async{
            let! cont = async{
                try
                    let! model = async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.loading)
                        return! load()
                    }
                    return this.ShowForm(model)
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.ShowForm(model) = async{
            let! cont = async{
                try
                    let! res = CertificatesView.Show(ctx, model)
                    return res.Handle(
                        upload = (fun filePath-> this.UploadCertificate(filePath)),
                        setStatus = (fun enabled-> this.SetCertifacateStatuses(model, enabled))
                    )
                with err -> 
                    do! show_error(err)
                    return this.ShowForm(model)
            }
            return! cont
        }
        
        member private this.UploadCertificate(filePath) = async{
            let! cont = async{
                try
                    do! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.uploading)
                        let cert = new Certificate()
                        let finfo = new FileInfo(filePath)
                        use fstream = finfo.OpenRead() 
                        let! data = fstream.AsyncRead(int(fstream.Length))
                        cert.CertificateID <- finfo.Name
                        cert.Certificate1 <- new BinaryData()
                        cert.Certificate1.Data <- data
                        do! dev.LoadCertificates([|cert|])
                    }
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        
        member private this.SetCertifacateStatuses(model, enabled) = async{
            let! cont = async{
                try
                    //let! certificates = dev.GetCertificates()
                    do! async{
                        use! progress = Progress.Show(ctx, LocalDevice.instance.applying)
                        let statuses = seq{
                            for i in model.enabled do
                                if enabled |> Seq.forall (fun e-> e <> i) then
                                    yield new CertificateStatus(
                                        CertificateID = i,
                                        Status = false
                                    )
                            for i in enabled do
                                if model.enabled |> Seq.forall (fun e-> e <> i) then
                                    yield new CertificateStatus(
                                        CertificateID = i,
                                        Status = true
                                    )
                        }
                        do! dev.SetCertificatesStatus(statuses |> Seq.toArray)
                    }
                    return this.Main()
                with err -> 
                    do! show_error(err)
                    return this.Main()
            }
            return! cont
        }
        

        member private this.Complete(res) = async{
            return res
        }
        

        static member Run(ctx:IUnityContainer) = 
            let act = new CertificateManagementActivity(ctx)
            act.Main()
    end
