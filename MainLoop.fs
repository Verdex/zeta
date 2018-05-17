
namespace Zeta

module MainLoop =

    open ConsoleTypes
    open ConsoleInterface
    open ConsoleReader
    open System.Threading

    open System

    let main = 
    
        Console.BackgroundColor <- ConsoleColor.Blue
        Console.ForegroundColor <- ConsoleColor.Green

        Console.Clear()

        Console.SetCursorPosition( 5, 5 )

        Console.Write( 'W' )
    
        //initConsoleInterface()
        //initConsoleReader( consoleMailbox  )

        let rec blah () =
            Thread.Sleep( 500 )
            let r = consoleMailbox.PostAndReply( fun rc -> GetLastNKeys( rc, 4 ) )
            printfn "%A" r
            match r with
                | [ KeyPressEvent( 'q', _ ) ; KeyPressEvent( 'u', _ ) ; KeyPressEvent( 'i', _) ; KeyPressEvent( 't', _) ] -> () 
                | _ -> blah ()
            blah()

        Console.WriteLine( "blah" )
        //blah()

        //killConsoleReader()

