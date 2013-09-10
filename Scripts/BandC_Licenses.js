google.load('visualization', '1', { 'packages': ['geochart'] });

google.setOnLoadCallback(drawRegionsMap);


function drawRegionsMap() {
    

//    var ServerData = google.visualization.arrayToDataTable([
//            ['State', 'PE Licenses', 'Master Electrician'],
//            ['TX', 15, 2],
//            ['CA', 3, 1],
//            ['IL', 4, 0],
//            ['IA', 5, 3],
//            ['OK', 6, 2],
//            ['FL', 7, 4],
//            ['AL', 0, 10]
//        ]);

    var ServerData = google.visualization.arrayToDataTable([
            ['State', 'PE Licenses', 'Master Electrician'],
            ['aa', 0, 0],
            ['xx', 15, 15]
        ]);

    ///////////////////
    // Configure Map //
    ///////////////////
    var MapOptions = {
        region: 'US',
        resolution: 'provinces',
        colorAxis: { colors: ['red', 'green'] }
    };

    //////////////////////////////////
    // Set Element To Load Map Into //
    //////////////////////////////////
    var GoogleMap = new google.visualization.GeoChart(document.getElementById('Google_Map_Div'));

    /////////////////////////////////////////////////////////////////
    // Catch Click Event So We Can Explode Licenses For That State //
    /////////////////////////////////////////////////////////////////
    google.visualization.events.addListener(GoogleMap, 'select', function () {
        var selectionIdx = GoogleMap.getSelection()[0].row;
        var stateName = ServerData.getValue(selectionIdx, 0);
        var value = data.getValue(selectionIdx, 1);

        window.open('http://seiadev.forumone.com/state-solar-policy/' + stateName);

    });

    //////////////////////////////////////////
    // Finally We ARe Ready To Draw The Map //
    //////////////////////////////////////////
    GoogleMap.draw(ServerData, MapOptions);
};






    

