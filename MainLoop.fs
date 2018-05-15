
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

        let r = consoleMailbox.PostAndReply( fun rc -> GetLastNKeys( rc, 4 ) )
        printfn "%A" r

        killConsoleReader()

