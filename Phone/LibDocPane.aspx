<!DOCTYPE html>
<html>
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
        <meta name="apple-mobile-web-app-capable" content="yes" />
        <meta name="apple-mobile-web-app-status-bar-style" content="black" />

        <title>
            Home Page Menu
        </title>
        
	    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.2.1/jquery.mobile-1.2.1.min.css" />
	    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.min.js"></script>
	    <script type="text/javascript"  src="http://code.jquery.com/mobile/1.2.1/jquery.mobile-1.2.1.min.js"></script>

        <script type="text/javascript"  src="/Scripts/Site.js" ></script>
        <script type="text/javascript"  src="/Phone/Scripts/SiLibrary.js" ></script>
        <script type="text/javascript"  src="/Phone/Scripts/LibDocPane.js" ></script>
        

        <style type="text/css">

            .ui-btn 
            {
            	text-align: left;
            }
            
            
            .ui-icon-doc {
	            background-image: url("/images/word_32.gif");
            }
            
            .ui-icon-docx {
	            background-image: url("/images/word_32.gif");
            }
            
            .ui-icon-pdf {
	            background-image: url("/images/pdf_18.gif");
            }            
                        
            @media only screen and (-webkit-min-device-pixel-ratio: 2) {
	            .ui-icon-doc {
	                background-image: url("/images/word_32.gif");
	                -ms-background-size: 18px 18px;
	                background-size: 18px 18px;
	            }

                .ui-icon-docx {
                    background-image: url("/images/word_32.gif");
                    -ms-background-size: 18px 18px;
                    background-size: 18px 18px;
                }

                .ui-icon-pdf {
                    background-image: url("/images/pdf_18.gif");
                    -ms-background-size: 18px 18px;
                    background-size: 18px 18px;
                }
            }
        
        </style>
    </head>
    

    <body>
        <!-- Home -->
        <div id="page1" data-role="page">
            <div data-role="header" data-theme="b">
                <h3>
                    Shermco YOU!
                </h3>
            </div>    
                
                
            <div data-role="content"> <!-- Start Content -->
                
	            <div data-role="header" data-theme="b">
		            <h1>Document Library</h1>
	            </div>
                     
                    <a id="BackPath" href="#" data-role="button" style='width: 130px;' data-mini="true" data-theme="a" data-icon="arrow-l">Back</a>
                    
                    <div class="NavSet" data-role="collapsible-set" data-theme="b" data-content-theme="c">  <!-- Start NavSet -->
                        
                        <div style="text-align: center;">
                            <img src="/images/slider-loading.gif" />
                            <h3>Loading...</h3>
                        </div>

                    
                    </div>  <!-- End NavSet -->

            
            </div>  <!-- End Content -->

            <div data-role="footer" data-theme="a" data-position="fixed">
                <h3>
                    Footer 1
                </h3>
            </div>
        </div>
    </body>
</html>
