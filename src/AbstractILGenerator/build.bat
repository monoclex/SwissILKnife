mkdir "lib" "lib/net45" "lib/netstandard2.0"
start ilasm abstractilgenerator-net.il /out:lib/net45/AbstractILGenerator.dll /dll
start ilasm abstractilgenerator-standard.il /out:lib/netstandard2.0/AbstractILGenerator.dll /dll
pause