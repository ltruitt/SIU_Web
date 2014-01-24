	<?php

function bytesToSize1024($bytes, $precision = 2) {
    $unit = array('B','KB','MB');
    return @round($bytes / pow(1024, ($i = floor(log($bytes, 1024)))), $precision).' '.$unit[$i];
}

$sFileName = $_FILES['txtFile']['name'];
$sFileType = $_FILES['txtFile']['type'];
$sFileSize = bytesToSize1024($_FILES['txtFile']['size'], 1);
$sTempName = $_FILES['txtFile']['tmp_name'];

$argument1 = $_GET['SUB'];
$target_path = "/files/" .$argument1 . "/" . $sFileName ;
$target_path = "e:\\documents\\UPLOADS\\" .$argument1 . "\\" . $sFileName ;



//echo <<<EOF
//	<p>$target_path</p>
//	<p>$sTempName</p>
//	<p>$argument1</p>
//EOF;

if(move_uploaded_file($sTempName, $target_path)) {
    //echo "The file ".  $sFileName. " was uploaded";
	echo "Success";
} else {
    echo "There was an error uploading the file!";
}



//echo <<<EOF
//<p>Type: {$sFileType}</p>
//<p>Size: {$sFileSize}</p>
//EOF;
