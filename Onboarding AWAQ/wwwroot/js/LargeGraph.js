/* Obtenemos el id de usuario */
fetch('/GetUserSession')
    .then(response => response.text())
    .then(result => {
        // Request to api
        fetch(`https://localhost:7117/api/Web/getHistroicPoints/${result}`, {
            method: 'GET',
            mode: "cors"
        })
            .then(response => response.json())
            .then(results => {
                const dates = [];
                const dateData = [];
                for (var i = 0; i < results.length; i++) {
                    getPoints(results[i], i + 1, dateData, dates);
                }
                graficateLarge(dateData, dates)
            })
            .catch(e => {
                console.log("Sin capacitacion inicial" + e)
                noInfoLarge();
            })
    });

function getPoints(pointsInfo, position, dateData, dates) {
    var date = pointsInfo["fecha"];
    var points = pointsInfo["puntos"];
    dates.push(date)
    dateData.push({ x: position * 2, y: points })
}

function noInfoLarge() {
    var div = document.querySelector("#graphL");
    div.innerHTML = "No se ha empezado la capacitacion";
}


const data2 = [
    { x: 1, y: 1000 },
    { x: 2, y: 1000 },
    { x: 3, y: 1550 },
    { x: 4, y: 950 },
    { x: 5, y: 1620 },
    { x: 6, y: 1580 },
    { x: 7, y: 640 },
    { x: 8, y: 1800 },
    { x: 9, y: 1880 },
    { x: 10, y: 2000 }
];

function graficateLarge() {
    // Set up the dimensions of the chart
    var margin = { top: 20, right: 30, bottom: 30, left: 60 },
        width = 960 - margin.left - margin.right,
        height = 240 - margin.top - margin.bottom;

    // Create the SVG container
    const svg2 = d3.select("#graphL").append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", `translate(${margin.left}, ${margin.top})`);

    // Define the scales for x and y axes
    const x = d3.scaleTime()
        .range([0, width])
        .domain(d3.extent(data2, d => new Date(d.x)));

    const y = d3.scaleLinear()
        .range([height, 0])
        .domain([0, d3.max(data2, d => d.y)]);

    // Create axes
    const xAxis = svg2.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x).tickSize(0).tickValues([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
            .tickFormat(function (d, i) {
                const dates = ["May 01", "May 15", "Jun 01", "Jun 15", "Jul 01", "Jul 15", "Aug 01", "Aug 15", "Sep 01"];
                return dates[i];
            })
            .tickPadding(16) // Add padding between the ticks and the text
        )
        .call(g => g.select(".domain").style("stroke", "#EDEDED").style("stroke-width", "2px"))
        .selectAll("text")
        .style("text-anchor", "start"); // Align the text to the left

    svg2.append("g")
        .call(d3.axisLeft(y).tickSize(0).tickPadding(16)
            .tickValues([0, 500, 1000, 1500, 2000]) // Specify the points at which ticks should be placed
            .tickFormat(function (d) { // Format the ticks
                if (d === 0) return "0";
                else if (d === 500) return "500";
                else if (d === 1000) return "1,000";
                else if (d === 1500) return "1,500";
                else return "2,000";
            })
        )
        .call(g => g.select(".domain").style("stroke", "#EDEDED").style("stroke-width", "2px"))
        .selectAll("text")
        .style("text-anchor", "end"); // Align the text to the left

    svg2.selectAll(".vertical-line")
        .data(d3.range(1, 10, 1)) // Define the range and step for the lines
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
    svg2.selectAll(".tick line").remove();

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
    const gradient2 = svg2.append("defs")
        .append("linearGradient")
        .attr("id", "area-gradient2")
        .attr("x1", "0%")
        .attr("y1", "0%")
        .attr("x2", "0%")
        .attr("y2", "100%");

    gradient2.append("stop")
        .attr("offset", "20%")
        .attr("stop-color", "#2CDF5E")
        .attr("stop-opacity", 0.2);

    gradient2.append("stop")
        .attr("offset", "100%")
        .attr("stop-color", "#2CDF5E")
        .attr("stop-opacity", 0);

    // Draw the area with the gradient fill
    svg2.append("path")
        .datum(data2)
        .attr("fill", "url(#area-gradient2)")
        .attr("d", area);

    // Draw the line
    svg2.append("path")
        .datum(data2)
        .attr("fill", "none")
        .attr("stroke", "#2CDF5E")
        .attr("stroke-width", 2)
        .attr("d", line);
}
