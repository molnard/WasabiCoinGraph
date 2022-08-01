## What is this? 

It is hard to see what is happening inside the wallet and the coins while you are CoinJoining. With this method described below, you will get a graph with arrows, how the coins and their anonymity score change with CoinJoin transactions. 

The original code is [here](https://github.com/lontivero/Wiki/blob/master/src/wasabi/tx_graph_generator.md), the purpose of this post is to help windows users. 

## Steps 

1. Clone this repository.
2. Start WSL. 
3. Start Wasabi with json RPC server on. https://docs.wasabiwallet.io/using-wasabi/RPC.html#configure-rpc
3. Run the following commands. 

./wcli.sh selectwallet <wallet-name>

delete coinlist.txt otherwise the next command will append.

./wcli.sh listcoins > coinlist.txt

cat coinlist.txt | dotnet fsi coinsgraph.fsx | sed -e 's/    ;//' | dot -Tpng > mywallet.png

the PNG will contain all the transactions and coins for the specified wallet. 
