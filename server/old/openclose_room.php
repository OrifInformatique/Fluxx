<?php 
	require('connection.php');

//ce fichier sert a fermer la room quand la partie est prète ou à la reouvrire si il y as un problème

	$idRoom = $_POST["room_id"];
	$open = $_POST["open"];

	$query = "UPDATE room SET open = $open WHERE id = '$idRoom'";

	if (mysqli_query($conn, $query)) {
		echo "successfully opened"; 
	}else{ 
		echo "error"; 
	}
	
mysqli_close($conn);
?>