// Setup basic express server
var express = require('express');
var app = express();
var path = require('path');
var server = app.listen(8080);
var io = require('socket.io').listen(server);
var port = process.env.PORT || 8080;
var mysql = require('mysql'); 
var _rooms_name = [];
var con = mysql.createConnection({
  host: "localhost",
  user: "root",
  password: "",
  database:"xamarinusers"
});

con.connect(function(err) {
  if (err) throw err;
  server.listen(port, () => {
    console.log('Server listening at port %d', port);
  });

  // Routing
  app.use(express.static(path.join(__dirname, 'public')));

  // Chatroom

  var numUsers = 0;

  io.on('connection', (socket) => {
    var addedUser = false;

    // when the client emits 'new message', this listens and executes
    socket.on('new message', (data) => {
      // we tell the client to execute 'new message'
      socket.broadcast.emit('new message', {
        username: socket.username,
        message: data
      });
      console.log(data.message);
    });

    //fonctionnel
    socket.on('login player', (player) => {
      console.log("->> createRoom:");
      con.query("SELECT * FROM `user` WHERE pseudo = \""+player.pseudo+"\" LIMIT 1", function (err, result, fields) {
        if (err) throw err;
        var return_value = "Error: error";
        if (result[0]!=undefined)
        {
          if (result[0].password == player.password)
          {
            return_value = result[0];
          } 
          else
          {
            return_value = "Error: Le mot de passe est incorrecte";
          } 
        }
        else
        {
          return_value = "Error: Ce pseudo n'existe pas";
        }
        console.log(player.pseudo+"->"+return_value+"\n\n");
        console.log([result[0], return_value]);
        socket.emit("login player echo", return_value);
       }); 
    });

    //...
    socket.on('connection player', (player) => {
      console.log("->> connectionPlayer:\n"+player.Id+", "+player.Pseudo+", "+player.Password+", "+player.Color+", "+player.Wins+", "+player.Losses+".")
    });

    socket.on('create room', room =>{
      console.log("->> createRoom:");
      console.log(_rooms_name); console.log(room.Name);
      //console.log(room);//console.log(io.sockets.adapter.rooms);
      if(!_rooms_name.includes(room.Name)){
        socket.join(room.Name);
        _rooms_name.push(room.Name);
        io.sockets.adapter.rooms[room.Name].Name = room.Name;          
        io.sockets.adapter.rooms[room.Name].Password = room.Password;
        io.sockets.adapter.rooms[room.Name].NumPlayer = room.NumPlayer;
        io.sockets.adapter.rooms[room.Name].Open = true;
        io.sockets.adapter.rooms[room.Name].Host = room.Host;
        io.sockets.adapter.rooms[room.Name].Players = new Array(room.NumPlayer-1);
        socket.emit("create room echo", true);
      }
      else{
        socket.emit("create room echo", false);
      }
    });

    socket.on('get all room', function(){
      console.log("->> getAllRoom:\n"+_rooms_name);
      var result = [];
      _rooms_name.forEach(room_name => {
        var room = [];
        room = io.sockets.adapter.rooms[room_name];
        console.log(io.sockets.adapter.rooms[room_name]);
        if(room!=null) result.push(room); 
      });
      socket.emit("get all room echo", result);
    });

    
    
    socket.on('join room', room =>{
      console.log("->> joinRoom");
      console.log(room);
      if(!(socket.rooms.indexOf(room.Name) >= 0)){
        if (io.socket.adapter.rooms[room.Name].Open){
          if (room.Password == io.socket.adapter.rooms[room.Name].Password){
            var rooms_players = io.socket.adapter.rooms[room.Name].Players;
            var i = 0;
            while(rooms_players[i] != undefined){
              i++;
            }
            if (i>rooms_players.length){ 
              socket.emit("join room echo", "Error: Cette room étais déjà pleine");
              io.socket.adapter.rooms[room.Name].Open = false;
            }else{
              socket.emit("join room echo", room);
              io.socket.adapter.rooms[room.Name].Players = player
              while(rooms_players[i] != undefined){
                i++;
              } 
              if(i>rooms_players.length){
                 io.socket.adapter.rooms[room.Name].Open = false;
              }
            }
          }else{
            socket.emit("join room echo", "Error: Ce mot de passe set faux");
          }
        }else{
          socket.emit("join room echo", "Error: Cette room est fermée");
        }
      }else{
        socket.emit("join room echo", "Error: Cette room n'existe pas");
      }  
    });
                                                                                                                     
    socket.on('leave room', (room) =>{
      console.log("->> leaveRoom");
      console.log(room);
      if (room.Host == undefined){
        _rooms_name.splice(_rooms_name.indexOf(room.Name), 1);
       
      }
      //on véra bien
    });

    //#region base
    // when the client emits 'add user', this listens and executes
    socket.on('add user', (username) => {
      //console.log("->> AddUser\n"+username);
      if (addedUser) return;

      // we store the username in the socket session for this client
      socket.username = username;
      ++numUsers;
      addedUser = true;
      socket.emit('login', {
        numUsers: numUsers
      });
      // echo globally (all clients) that a person has connected
      socket.broadcast.emit('user joined', {
        username: socket.username,
        numUsers: numUsers
      });
    });

    // when the client emits 'typing', we broadcast it to others
    socket.on('typing', () => {
      socket.broadcast.emit('typing', {
        username: socket.username
      });
    });

    // when the client emits 'stop typing', we broadcast it to others
    socket.on('stop typing', () => {
      socket.broadcast.emit('stop typing', {
        username: socket.username
      });
    });

    // when the user disconnects.. perform this
    socket.on('disconnect', () => {
      if (addedUser) {
        --numUsers;

        // echo globally that this client has left
        socket.broadcast.emit('user left', {
          username: socket.username,
          numUsers: numUsers
        });
      }
    });
    //#endregion

  });
});