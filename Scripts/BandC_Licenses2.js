google.load('visualization', '1', { 'packages': ['geochart'] });



$(document).ready(function(){
    google.setOnLoadCallback(drawRegionsMap);
});









function drawRegionsMap() {
    
    ////////////////////////////////
    // Show Sum Of Hours For Week //
    ////////////////////////////////
    
    function GetSumRpt() {
        var GetSumRpt_ajax = new AsyncServerMethod();
        GetSumRpt_ajax.exec("/SIU_DAO.asmx/ESD_License_Sum", GetSumRpt_success);
    }

    function GetSumRpt_success(data) {

        parsedData = $.parseJSON(data.d);

        
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'State');
        data.addColumn('number', 'PE Licenses');
        data.addColumn('number', 'Master Electricians');
        data.addRows(parsedData.length + 2);

        for (var idx = 0; idx < parsedData.length; idx++) {
            data.setCell(idx, 0, parsedData[idx].State);
            data.setCell(idx, 1, parsedData[idx].Professional);
            data.setCell(idx, 2, parsedData[idx].Master);
        }

        data.setCell(idx, 0, 'xx');
        data.setCell(idx, 1, 0);
        data.setCell(idx, 2, 0);
        idx++;
        data.setCell(idx, 0, 'yy');
        data.setCell(idx, 1, 5);
        data.setCell(idx, 2, 5);        
        
        ///////////////////
        // Configure Map //
        ///////////////////
        var MapOptions = {
            region: 'US',
            resolution: 'provinces',
            colorAxis: { colors: ['red', 'green'] }
        };

        /////////////////////////////////////////////////////////////////
        // Catch Click Event So We Can Explode Licenses For That State //
        /////////////////////////////////////////////////////////////////
        google.visualization.events.addListener(GoogleMap, 'select', function () {
            var selectionIdx = GoogleMap.getSelection()[0].row;
            var stateName = ServerData.getValue(selectionIdx, 0);
            var value = data.getValue(selectionIdx, 1);

            window.open('http://seiadev.forumone.com/state-solar-policy/' + stateName);

        });
        

        
        GoogleMap.draw(data, MapOptions);
    }
    GetSumRpt();
    




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



    //////////////////////////////////////////
    // Finally We ARe Ready To Draw The Map //
    //////////////////////////////////////////
    GoogleMap.draw(ServerData, MapOptions);
};






    

