
cc = fsharpc

all : 
	$(cc) ConsoleInteract.fs MainLoop.fs

clean :
	rm -rf *.exe
