open System
open System.IO

type Row = { tx: string; idx: int; amount:int64; anonset:float; confirmed:bool; confirmations:int; address:string; spentBy:string; path:string}
let toGraph (tx: string * Row list) : string =
  let txid, outs = tx
  let anonsets = outs |> List.map (fun x -> string x.anonset) |> String.concat "|"
  let amounts = outs |> List.map (fun x -> $"<idx{x.idx}>{x.amount}") |> String.concat "|"
  let outputs = $"{{ {{ PrvScore | {anonsets} }} | {{ Amount | {amounts} }} }}"
  let edges   = outs |> List.filter (fun x -> x.spentBy <> "") |> List.map (fun x -> $"tx{x.tx}:idx{x.idx} -> tx{x.spentBy}") |> String.concat ";\n    "
  $"tx{txid} [label=\"Tx Id: {txid[..8]}|{{{{ {outputs} }} }}}}\"];\n    {edges}"

let tableArray = 
  File.ReadAllLines("coinlist.txt") 
  |> Array.toList 
  |> List.tail 
  |> List.map (fun l -> l.Split(" ", StringSplitOptions.RemoveEmptyEntries))
  |> List.map (fun e -> { tx = e[0]; idx = int e[1]; amount = int64 e[2]; anonset = float e[3]; confirmed = (if e[4] = "true" then true else false); confirmations = int e[5]; address = e[7]; path = e[6]; spentBy = (if e.Length = 9 then e[8] else "") })
  |> List.groupBy (fun x -> x.tx) 
  |> List.map toGraph 

let str = $"""
digraph G {{
    graph [center=1 rankdir=LR; overlap=false; splines=true;];
    edge [dir=forward];
    node [shape=record; ordering="in" ];
    {tableArray |> String.concat ";\n    "};
}}"""

Console.WriteLine(str)