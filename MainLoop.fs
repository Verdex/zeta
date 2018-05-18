
namespace Zeta

module MainLoop =

    open ConsoleTypes
    open ConsoleInterface
    open ConsoleReader
    open System.Threading

    open System

    let main = 
    
        initConsoleInterface()
        initConsoleReader( consoleMailbox  )

        consoleMailbox.Post( WriteToConsole( [( {Char = 'w'; ForeColor = ConsoleColor.Blue; BackColor = ConsoleColor.Red }, 
                                                { X = 3 ; Y = 7 } );
                                                ( {Char = 'A'; ForeColor = ConsoleColor.Green; BackColor = ConsoleColor.Yellow }, 
                                                  { X = 5; Y = 10 } )] ) )

        let rec blah () =
            Thread.Sleep( 500 )
            let r = consoleMailbox.PostAndReply( fun rc -> GetLastNKeys( rc, 4 ) )
            printfn "%A" r
            match r with
                | [ KeyPressEvent( 'q', _ ) ; KeyPressEvent( 'u', _ ) ; KeyPressEvent( 'i', _) ; KeyPressEvent( 't', _) ] -> () 
                | _ -> blah ()
            blah()

        Console.WriteLine( "blah" )
        let x = getConsoleHeight()
        let y = getConsoleWidth() 
        printfn "h = %A ; w = %A" x y  
        //blah()

        killConsoleReader()

