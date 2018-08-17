<?php 
/*	require('connection.php');

//ce fichiuer sert à créer une nouvelle room avec les infos primaire et retourne la room crée.

	$roomName = $_POST["room_name"];
	$roomPass = $_POST["room_pass"];
	$playerID = $_POST["player_id"];

	$roomNumPlayer = $_POST["room_num_player"];

	$query = "INSERT INTO room (name, password, fk_host, player_number, open) values('$roomName', '$roomPass', '$playerID', '$roomNumPlayer', 0)";

	if (mysqli_query($conn, $query)) {
		
		$result = mysqli_insert_id($conn);
	
		$array['Id'] = $result;
		$array['Name'] = $roomName;
		$array['Password'] = $roomPass;
		$array['NumPlayer'] = $roomNumPlayer;
		$array['IdHost'] = $playerID;
		
		echo json_encode($array);
	}else{ 
		echo "error"; 
	}

mysqli_close($conn);*/
?>