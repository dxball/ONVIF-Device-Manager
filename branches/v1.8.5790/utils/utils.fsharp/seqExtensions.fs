module Seq
    ///<summary></summary>
    let inline repeat times sequence = seq{
        for i=1 to int(times) do
            yield! sequence
    }

