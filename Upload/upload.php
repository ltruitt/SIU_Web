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
$target_path = "e:\\documents\\UPLOADS\\" .$argument1 . "\\" . $sFileName ;

//echo <<<EOF
//	<p>1  $sFileName</p>
//	<p>2 $target_path</p>
//	<p>3 $sTempName</p>
//	<p>4 $argument1</p>
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
