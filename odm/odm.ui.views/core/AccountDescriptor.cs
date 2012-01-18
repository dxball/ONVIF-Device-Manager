using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using utils;
using System.Xml.Serialization;

namespace odm.ui.core {
    [XmlRootAttribute(ElementName = "Account", IsNullable = false)]
    public class odmAccount:DependencyObject {
		public odmAccount() {
        }
        public string Password { get; set; }

        public string Name {get { return (string)GetValue(NameProperty); }set { SetValue(NameProperty, value); }}
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(odmAccount));
    }

	public interface IAccount {
		odmAccount Account { get; }
		string Name { get; set; }
		string Password { get; set; }
	}
	public class AccountDescriptor:IAccount {
		public AccountDescriptor(odmAccount acc) {
			Account = acc;
		}
		odmAccount account;
		public odmAccount Account {
			get { 
				return account;
			}
			private set { account = value; }
		}
		public string Name {
			get { return account.Name; }
			set {	account.Name = value; }
		}
		public string Password {
			get { return account.Password; }
			set { account.Password = value; }
		}
	}
	public class DefAccountDescriptor:IAccount {
		public odmAccount Account {
			get { 
				return null; 
			}
		}
		public string Name {
			get { return ""; }
			set { dbg.Assert(true); }
		}
		public string Password {
			get { return ""; }
			set { dbg.Assert(true); }
		}
	}

    public class AccountManager {
        public static IAccount CurrentAccount {
            get {
                if (!File.Exists(settingsPath))
                    return new DefAccountDescriptor();
                try {
                    using (var sr = File.OpenText(settingsPath)) {
                        XmlSerializer deserializer = new XmlSerializer(typeof(odmAccount));
                        odmAccount acc;
                        acc = (odmAccount)deserializer.Deserialize(sr);
						IAccount iacc = (acc.Name == "" && acc.Password == "") ? (IAccount)new DefAccountDescriptor() : (IAccount)new AccountDescriptor(acc);
                        return iacc;
                    }
                } catch (Exception err) {
                    dbg.Error(err);
					return new DefAccountDescriptor();
                }
            }
        }
        private static void SaveCurrent(odmAccount account) {
            try {
                if(File.Exists(settingsPath))    
                    File.Delete(settingsPath);
				if (account != null) {
					using (var sr = File.CreateText(settingsPath)) {
						XmlSerializer serializer = new XmlSerializer(typeof(odmAccount));
						serializer.Serialize(sr, account);
					}
				}
            } catch (Exception err) {
                dbg.Error(err);
            }

        }
        public static void SetCurrent(IAccount account) {
			try {
				if (account == null) {
					SaveCurrent(new odmAccount() { Name = "", Password = "" });
				} else
					SaveCurrent(account.Account);
			} catch (Exception err) {
				dbg.Error(err.Message);
			}
        }
        static string settingsPath = AppDefaults.ConfigFolderPath + "account.def.xml";
        static string path = AppDefaults.ConfigFolderPath + "account.info.xml";
        public static void Save(List<IAccount> acclist) {
            try {
                if(File.Exists(path))
                    File.Delete(path);

				var tmpAcc = new List<odmAccount>();
				acclist.ForEach(acc => {
					tmpAcc.Add(acc.Account);
				});

                using (var sr = File.CreateText(path)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<odmAccount>));

					serializer.Serialize(sr, tmpAcc);
                }
            } catch (Exception err) {
                dbg.Error(err);
            }
        }
        public static List<IAccount> Load() {
            if (!File.Exists(path))
                return new List<IAccount>();
            try {
                using (var sr = File.OpenText(path)) {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<odmAccount>));
                    List<odmAccount> acclst = (List<odmAccount>)deserializer.Deserialize(sr);
					List<IAccount> iacclst = new List<IAccount>();
					acclst.ForEach(acc => {
						iacclst.Add(new AccountDescriptor(acc));
					});
                    return iacclst;
                }
            } catch (Exception err) {
                dbg.Error(err);
                return new List<IAccount>();
            }
        }
    }
}
