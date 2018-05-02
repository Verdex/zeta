
namespace Zeta

module MainLoop =

    open ConsoleReader
    open System.Threading

    let main = 
        initConsoleReader()

        Thread.Sleep( 10000 )


