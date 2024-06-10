/* Obtenemos el id de usuario */
fetch('/GetUserSession')
.then(response => response.text())
.then(result => {
    // Request to api
    fetch(`https://localhost:7117/api/Web/getZonePoints/${result}`, {
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
    .catch(e => {
        zones = ["Rio", "Bosque", "Ciudad", "Montaña"];
        graficate(zoneData, zones);
    })
});

function getData(zoneInfo, position, zoneData, zones) {
    var zone = zoneInfo["zona"];
    var points = zoneInfo["puntos"];
    var completion = zoneInfo["progreso"];
    zones.push(zone)
    zoneData.push({x : position * 2, y : points})
}

function noInfo() {
    var div = document.querySelector("#graph");
    var pInvalid = document.createElement("p");
    pInvalid.innerHTML = "N/A";
    pInvalid.classList.add("time-kpi")

    var pText = document.createElement("p");
    pText.innerHTML = "No se a comenzado la capacitacion";
    pText.classList.add("time-unit")
    div.append(pInvalid, pText);
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
    const x = d3.scaleBand()
        .range([0, width])
        .domain(data.map(d => d.x))
        .padding(0.1);

    const y = d3.scaleLinear()
        .range([height, 0])
        .domain([0, d3.max(data, d => d.y)]);

    // Create axes
    const xAxis = svg.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x).tickSize(0).tickPadding(16)
            .tickFormat(function (d, i) {
                return zones[i].split(" - ")[0];
            })
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

    // Draw the bars
    svg.selectAll(".bar")
        .data(data)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", d => x(d.x))
        .attr("width", x.bandwidth())
        .attr("y", d => y(d.y))
        .attr("height", d => height - y(d.y))
        .attr("fill", "#FF986B");
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