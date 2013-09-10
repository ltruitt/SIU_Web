<%--http://videojs.com/--%>
<%--http://wiki.answers.com/Q/What_format_of_video_does_the_iphone_play--%>
<%--http://www.html5rocks.com/en/tutorials/video/basics/--%>

<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
        <meta name="apple-mobile-web-app-capable" content="yes" />
        <meta name="apple-mobile-web-app-status-bar-style" content="black" />

        <title>
            Phone Video
        </title>
        
        
        <script src="http://s3.amazonaws.com/codiqa-cdn/jquery-1.7.2.min.js" type="text/javascript"></script>
        <link href="https://s3.amazonaws.com/codiqa-cdn/mobile/1.2.0/jquery.mobile-1.2.0.min.css" rel="stylesheet"  />
        <script src="http://s3.amazonaws.com/codiqa-cdn/mobile/1.2.0/jquery.mobile-1.2.0.min.js" type="text/javascript"></script>
        
<%--        <link href="http://vjs.zencdn.net/c/video-js.css" rel="stylesheet">
        <script src="http://vjs.zencdn.net/c/video.js" type="text/javascript"></script>--%>
    </head>
    
    
    
    

    <body>
        <!-- Home -->
        <div id="page1" data-role="page">
            <div data-role="header" data-theme="b">
                <h3>
                    Shermco YOU!
                </h3>
                <h2>
                    Video
                </h2>
            </div>
            


<video id="Video1" class="video-js vjs-default-skin" controls
  preload="auto" width="640" height="264" poster="/Images/Si-Shermco-OneLine.png"
  data-setup="{}">
  <source src="http://www.html5rocks.com/en/tutorials/video/basics/Chrome_ImF.mp4" type='video/mp4; codecs="avc1.42E01E, mp4a.40.2"'>
</video>




                

            <div data-role="footer" data-theme="a" data-position="fixed">
                <h3>
                    Footer 2
                </h3>
            </div>

        </div>
    </body>
</html>