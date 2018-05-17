
namespace Zeta

module MainLoop =

    open ConsoleTypes
    open ConsoleInterface
    open ConsoleReader
    open System.Threading

    let main = 
        initConsoleInterface()
        initConsoleReader( consoleMailbox  )

        let rec blah () =
            Thread.Sleep( 500 )
            let r = consoleMailbox.PostAndReply( fun rc -> GetLastNKeys( rc, 4 ) )
            printfn "%A" r
            match r with
                | [ KeyPressEvent( 'q', _ ) ; KeyPressEvent( 'u', _ ) ; KeyPressEvent( 'i', _) ; KeyPressEvent( 't', _) ] -> () 
                | _ -> blah ()
            blah()

        blah()

        killConsoleReader()

