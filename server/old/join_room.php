<?php 
	require('connection.php');

// ce fichier sert a connecter un joueurs via son id à une room via son id, il marque l'id du joueur dans la room et change le statut inRoom du joueur.

	$idRoom = $_POST["room_id"];
	$idPlayer = $_POST["player_id"];

	$query = "UPDATE player SET inRoom = 0 WHERE id = $idPlayer";

	if (mysqli_query($conn, $query)) {
		//echo "successfully"; 
	}else{ 
		echo "error: change player status \"inRoom\""; 
	}


	$query = "SELECT fk_player_2, fk_player_3, fk_player_4, fk_player_5, fk_player_6 FROM room WHERE id = '$idRoom'";

	$result = mysqli_query($conn, $query); 

	     if (is_null($result->fetch_assoc()["fk_player_2"])){$fk_player = "fk_player_2";}
	else if (is_null($result->fetch_assoc()["fk_player_3"])){$fk_player = "fk_player_3";}
	else if (is_null($result->fetch_assoc()["fk_player_4"])){$fk_player = "fk_player_4";}
	else if (is_null($result->fetch_assoc()["fk_player_5"])){$fk_player = "fk_player_5";}
	else if (is_null($result->fetch_assoc()["fk_player_6"])){$fk_player = "fk_player_6";}
	else { echo "error: no free space found"; }

	$query = "UPDATE room SET $fk_player = $idPlayer WHERE id = $idRoom";

	if (mysqli_query($conn, $query)) {
		echo "successfully"; 
	}else{ 
		echo "error: set fk in room"; 
	}

mysqli_close($conn);
?>