-==========================================-
Josh Oblinsky, Ryan Kingston
12/1/2014
Boggle Game
-==========================================-
*
*
*
|--------------------------------|Files|--------------------------------|

This solution contains the following files:

- boggleServer : Contains all information for building the connections among the clients to the server.
					this project also handles all console outputs and game states.

- BoggleClient : The client GUI. Contains the model, view, and controller that will build and update the GUI.

- Launcher : Runs a game of two on the local computer. Used mainly for testing and quick demonstrations.

- README : Contains all documentation for developers.

|-----------------------------------------------------------------------|
*
*
*
|--------------------------------|HOW TO PLAY|--------------------------------|

To play, run the launcher from Visual Studio. You will be prompted to enter a user name for each player, as well as an IP address. Use localhost.

The rules of the game will be displayed in the game window.

|-----------------------------------------------------------------------|
*
*
*
|=========================Project Documentation=========================|

- Player 1 holds the connection between server and player 1.
- Player 2 holds the connection between server and player 2.
- The server is a TCP Listener, and contains the connections between itself and all of the clients.

- When the number of players in the queue is divisible by 2, the program will dequeue 2 players, and
  create a game session using them. A boggle board is created for the players, as well.

- Upon connecting to the server, the client will send it's name to the server.
- Upon receiving a connection from 2 clients, the server will send the string "START $ # @", where $ is the 
  optional 16 character board, # is the game time in seconds, and @ is the opponent's name.
- Afterwards, the server will build a game session, and pass the players and board to the game session.
- The server will then await new.

- Should a player leave a session, the remaining player will be notified, and the socket connection will be
  closed.

- Player class contains a method to send the IGNORING [message] message to the user when they
  input an invalid command.

- If the server receives a null string, that indicates that it has lost connection with the client
  that sent it. This will allow the server to know when to close a game session.

- The client consists of 3 tabs: the main menu, the game screen, and the game summary screen.

- The main menu tab gives the player the ability to enter their name, and the IP address of the server they wish to connect to.

- The game screen allows the player to play a game of boggle against an opponent. They have the option to quit the game they are currently in, which will return them to the main menu tab.

- The game summary screen allows the player to view the end results of the game they just finished playing. They can view things such as the valid words played by each player, the words that they
  both played, and their invalid words. The player can then return to the main menu to play another game.

- If a player leaves a game session, the other player will be informed of this, and will be returned to the main menu.

- If the server gets disposed of when players are connected to it, each player will be informed that the server has shut down, and be returned to the main menu.

- The client updates the GUI using event handlers provided by the controller. The model will be the part that fires off the events that will call the event handlers.

|=======================================================================|


%-------------------------------------%
Time Log:

Monday:    3:00pm  - 5:45pm (2 3/4 hours)
Tuesday:   10:30am - 1:30pm (3 hours)
Wednesday: 4:00pm  - 5:45pm (1 3/4 hours)
Thursday:   3:30am - 7:30pm (4 hours)
Friday:    1:00pm  - 5:30pm (4 1/2 hours)

Sum: 16
%-------------------------------------%
