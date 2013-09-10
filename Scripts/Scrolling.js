// html

//<script src="/Scripts/jquery-scroller-v1.min.js" type="text/javascript"></script> 

//<style type="text/css">

//	/* CSS for the scrollers */
//	div.horizontal_scroller, div.vertical_scroller{
//		position:relative;
//		height:24px;
//		width:  99%;
//		display: block;
//		overflow:hidden;
//		border:#CCCCCC 1px solid;
//	    background-color: black;
//	}
//	div.scrollingtext{
//		position:absolute;
//		white-space:nowrap;
//		font-family:'Trebuchet MS',Arial;
//		font-size:18px;
//		font-weight:bold;
//		color:#000000;
//	}

//	/* style needed only for this example */
//	span#scrollingtext{margin:5px;padding:8px 12px;width:150px;background-color:#0066CC;color:#FFFFFF;border:#003366 1px solid;display:block;float:left;text-align:center;}
//	span#scrollingtext:hover{background-color:#0099CC}
//	#soccer_ball_container{width:99%;height:80px;margin: 0px;padding: 0px;}
//	#soccer_ball_container .scrollingtext{width:40px;height:80px;}
//	#soccer_ball{display:block;position:absolute;top:0px;left:0px;}

//</style>

//<div class="horizontal_scroller" id="soccer_ball_container">
//	<div class="scrollingtext">
//		<img src="/Images/soccer_ball.gif" alt="Scrolling Container Example 1" id="soccer_ball" />
//	</div>
//</div>
//    
//<br/>
//    
//<div>
//    <div class="horizontal_scroller" id="__no_mouse_events" style="height: 40px; width: 78%; float: left;">
//	    <div class="scrollingtext" style="color: white; vertical-align: middle;">
//	        <img src="/Images/soccer_ball.gif" alt="Scrolling Container Example 1"  />
//            <span style="margin-top: -10px;"><b>Scrolling text that pauses on mouse over and resumes on mouse out...</b></span>
//            <img src="/Images/soccer_ball.gif" alt="Scrolling Container Example 1"  />
//	    </div>
//    </div>

//    <div class="vertical_scroller" style="width:150px; height:100px; float: right;">
//	    <div class="scrollingtext" style="margin-left:20px; color: white;">
//		    Some<br />
//		    vertical<br />
//		    scrolling<br />
//		    text!
//	    </div>
//    </div>
//</div>



    $(document).ready(function () {

        //create scroller for each element with "horizontal_scroller" class...
        $('.horizontal_scroller').SetScroller({ velocity: 60,
            direction: 'horizontal',
            startfrom: 'right',
            loop: 'infinite',
            movetype: 'linear',
            onmouseover: 'pause',
            onmouseout: 'play',
            onstartup: 'play',
            cursor: 'pointer'
        });
        /*
        All possible values for options...
		
        velocity: 		from 1 to 99 								[default is 50]						
        direction: 		'horizontal' or 'vertical' 					[default is 'horizontal']
        startfrom: 		'left' or 'right' for horizontal direction 	[default is 'right']
        'top' or 'bottom' for vertical direction	[default is 'bottom']
        loop:			from 1 to n+, or set 'infinite'				[default is 'infinite']
        movetype:		'linear' or 'pingpong'						[default is 'linear']
        onmouseover:	'play' or 'pause'							[default is 'pause']
        onmouseout:		'play' or 'pause'							[default is 'play']
        onstartup: 		'play' or 'pause'							[default is 'play']
        cursor: 		'pointer' or any other CSS style			[default is 'pointer']
        */

        //how to overwrite options after setup and without deleting the other settings...
        $('#no_mouse_events').ResetScroller({ onmouseover: 'play', onmouseout: 'play' });
        $('#scrollercontrol').ResetScroller({ velocity: 85, startfrom: 'left' });

        //if you need to remove the scrolling animation, uncomment the following line...
        //$('#scrollercontrol').RemoveScroller();

        //how to play or stop scrolling animation outside the scroller... 
        $('#play_scrollercontrol').mouseover(function () { $('#scrollercontrol').PlayScroller(); });
        $('#stop_scrollercontrol').mouseover(function () { $('#scrollercontrol').PauseScroller(); });

        //create a vertical scroller...	
        $('.vertical_scroller').SetScroller({ velocity: 80, direction: 'vertical' });

        //"$('#soccer_ball_container')" inherits scrolling options from "$('.horizontal_scroller')",
        // then I reset new options; besides, "$('#soccer_ball_container')" will wraps and scrolls the bouncing animation...
        $('#soccer_ball_container').ResetScroller({ velocity: 85, movetype: 'pingpong', onmouseover: 'play', onmouseout: 'play' });

        //create soccer ball bouncing animation...
        $('#soccer_ball').bind('bouncer', function () {
            $(this).animate({ top: 42 }, 500, 'linear').animate({ top: 5 }, 500, 'linear', function () { $('#soccer_ball').trigger('bouncer'); });
        }).trigger('bouncer');

    });
