<?php 
	require('connection.php');

// ce fichier sert a supprimer une room a la fin d'une partie, elle supprime le statut inRoom de chaque joueurs

	$roomID = $_POST["room_id"];
	$id_invite = $_POST["id_invite"];
	$id_p2 = $_POST["id_p2"];
	$id_p3 = $_POST["id_p3"];
	$id_p4 = $_POST["id_p4"];
	$id_p5 = $_POST["id_p5"];
	$id_p6 = $_POST["id_p6"];
	$query = "UPDATE player SET inRoom = 0 WHERE id IN ('$id_invite','$id_p2','$id_p3','$id_p4','$id_p5','$id_p6')";

	if (mysqli_query($conn, $query)) {
		echo "players removed, \n";
	}else{ 
		echo "player remove error, \n"; 
	}

	$query = "DELETE FROM room WHERE id = $roomID";

	if (mysqli_query($conn, $query)) {
		echo "room deleted.";
	}else{ 
		echo "room delete error."; 
	}

mysqli_close($conn);
?>