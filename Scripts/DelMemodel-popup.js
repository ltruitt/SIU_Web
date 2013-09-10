jQuery.noConflict();
(
function($K){

$K.fn.Init=function(data)
    {
    /*
    Parameters: 
        popupCSS,
        closeButtonCSS,
        
    */
        $K(this).each(function(i)
            {
                var dbBack,intTopAxis=0;
                var objCloseButton= $K("." + data.closeButtonCSS);
                var objPopup= $K(this);                               
                var a;
            
                objCloseButton.click(function()
                    {
                        HidePopup();
                    });
            
                
                $K(window).scroll(function()
                {   
                    var xTop= parseInt($K(window).scrollTop()) + intTopAxis ;
                    objPopup.animate({top:xTop+ "px"},{queue: false, duration: 350});
                });
            
                
                initBackGround = function()
                {                   
                    dbBack = $K("<div></div>").attr("class","popupbackGround").css("height",$K(document).height()).hide();
                    $K("body").append(dbBack);
                    intTopAxis= parseInt(($K(window).height())/2)-(objPopup.height()/2);
                }
            

                ShowPopup = function()
                {
                    initBackGround();
                    dbBack.fadeIn(function(){objPopup.show();});
                    objPopup.css({"left":(($K(window).width())/2)-(objPopup.width()/2),"top":(($K(window).height())/2)-(objPopup.height()/2)+parseInt($K(window).scrollTop())});
                }


                HidePopup = function() {
                    objPopup.fadeOut();
                    dbBack.fadeOut();
                }
            

                ShowPopup();              
            });
    }

})(jQuery);