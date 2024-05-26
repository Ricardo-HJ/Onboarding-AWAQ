
// Request to api
fetch("https://localhost:7117/api/Web/getZonePoints/1", {
    method: 'GET',
    mode: "cors"
})
.then(response => response.json())
.then(results => {
    const zones = [];
    const zoneData = [];
    for (var i = 0; i < results.length; i++) {
        getData(results[i], i + 1, zoneData, zones);
    }
    graficate(zoneData, zones)
})

function getData(zoneInfo, position, zoneData, zones) {
    var zone = zoneInfo["zona"];
    var points = zoneInfo["puntos"];
    var completion = zoneInfo["progreso"];
    zones.push(zone)
    zoneData.push({x : position * 2, y : points})
}

function graficate(data, zones) {
    // Set up the dimensions of the chart
    var margin = { top: 20, right: 30, bottom: 30, left: 60 },
        width = 400 - margin.left - margin.right,
        height = 147 - margin.top - margin.bottom;

    // Create the SVG container
    const svg = d3.selectAll("#graph").each(function () {
        var svg = d3.select(this).append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
            .append("g")
            .attr("transform", `translate(${margin.left}, ${margin.top})`);

        // Define the scales for x and y axes
        const x = d3.scaleLinear()
            .range([0, width])
            .domain(d3.extent(data, d => d.x));

        const y = d3.scaleLinear()
            .range([height, 0])
            .domain([0, d3.max(data, d => d.y)]);

        // Create axes
        const xAxis = svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x).tickSize(0).tickValues(getDistanceX(data))
                .tickFormat(function (d, i) {
                    return zones[i].split(" - ")[0];
                })
                .tickPadding(16) // Add padding between the ticks and the text
            )
            .call(g => g.select(".domain").style("stroke", "#EDEDED").style("stroke-width", "2px"))
            .selectAll("text")
            .style("text-anchor", "start"); // Align the text to the left

        svg.append("g")
            .call(d3.axisLeft(y).tickSize(0).tickPadding(16)
                .tickValues(getDistanceY(data)) // Specify the points at which ticks should be placed
                .tickFormat(function (d) { // Format the ticks
                    return d + " pts"
                })
            )
            .call(g => g.select(".domain").style("stroke", "#EDEDED").style("stroke-width", "2px"))
            .selectAll("text")
            .style("text-anchor", "end"); // Align the text to the left

        svg.selectAll(".vertical-line")
            .data(d3.range(3, 11, 2)) // Define the range and step for the lines
            .enter()
            .append("line")
            .attr("class", "vertical-line")
            .attr("x1", d => x(new Date(d)))
            .attr("x2", d => x(new Date(d)))
            .attr("y1", 0)
            .attr("y2", height)
            .attr("stroke", "#EDEDED")
            .attr("stroke-width", 2)
            .attr("stroke-dasharray", "4,4");

        // Remove all ticks
        svg.selectAll(".tick line").remove();

        // Style the text
        d3.selectAll("text")
            .style("fill", "var(--Dark-grey, #525252)")
            .style("font-family", "Inter")
            .style("font-size", "12px")
            .style("font-style", "normal")
            .style("font-weight", "400")
            .style("line-height", "normal");

        // Define the line generator with a curved line
        const line = d3.line()
            .x(d => x(d.x))
            .y(d => y(d.y))
            .curve(d3.curveBasis);

        // Define the area generator with a curved area
        const area = d3.area()
            .x(d => x(d.x))
            .y0(height)
            .y1(d => y(d.y))
            .curve(d3.curveBasis);

        // Create the gradient for the area fill
        const gradient = svg.append("defs")
            .append("linearGradient")
            .attr("id", "area-gradient")
            .attr("x1", "0%")
            .attr("y1", "0%")
            .attr("x2", "0%")
            .attr("y2", "100%");

        gradient.append("stop")
            .attr("offset", "20%")
            .attr("stop-color", "#FF986B")
            .attr("stop-opacity", 0.2);

        gradient.append("stop")
            .attr("offset", "100%")
            .attr("stop-color", "#FF986B")
            .attr("stop-opacity", 0);

        // Draw the area with the gradient fill
        svg.append("path")
            .datum(data)
            .attr("fill", "url(#area-gradient)")
            .attr("d", area);

        // Draw the line
        svg.append("path")
            .datum(data)
            .attr("fill", "none")
            .attr("stroke", "#FF986B")
            .attr("stroke-width", 2)
            .attr("d", line);
    });
}

function getDistanceX(data) {
    var max = 0;
    for (var i = 0; i < data.length; i++) {
        if (max < data[i].x) { max = data[i].x };
    }
    var increment = max / (data.length * 1);
    max = max - (increment/ 2);
    increment = max / (data.length * 1);
    var ticks = [];
    for (var i = 0; i < data.length; i++) {
        ticks.push(increment * (i + 1));
    }
    console.log(ticks);
    return ticks;
}

function getDistanceY(data) {
    var max = 0;
    for (var i = 0; i < data.length; i++) {
        if (max < data[i].y) { max = data[i].y };
    }
    var increment = max / data.length;
    var ticks = [0]
    for (var i = 0; i < data.length; i++) {
        ticks.push(increment * (i + 1));
    }
    return ticks;
}