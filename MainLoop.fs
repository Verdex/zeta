
namespace Zeta

module MainLoop =

    open ConsoleTypes
    open ConsoleInterface
    open ConsoleReader
    open System.Threading

    let main = 
        initConsoleInterface()
        initConsoleReader( consoleMailbox  )

        Thread.Sleep( 5000 )

        let r = consoleMailbox.PostAndReply( fun rc -> GetNKeys( rc, (uint32)1 ) )
        printfn "%A" r
