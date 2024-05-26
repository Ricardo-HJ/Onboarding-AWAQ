// Sample data
fetch("", {
    method: "GET"
})

const data = [
    { x: 1, y: 0.5 },
    { x: 2, y: 1 },
    { x: 3, y: 0.75 },
    { x: 4, y: 1.25 },
    { x: 5, y: 0.8 },
    { x: 6, y: 1.1 },
    { x: 7, y: 1 },
    { x: 8, y: 1.8 },
    { x: 9, y: 1.4 },
    { x: 10, y: 2 }
];

// Set up the dimensions of the chart
var margin = {top: 20, right: 30, bottom: 30, left: 60},
    width = 400 - margin.left - margin.right,
    height = 147 - margin.top - margin.bottom;

// Create the SVG container
const svg = d3.selectAll("#graph, #graph2").each(function() {
    var svg = d3.select(this).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
    .append("g")
        .attr("transform", `translate(${margin.left}, ${margin.top})`);

// Define the scales for x and y axes
const x = d3.scaleTime()
    .range([0, width])
    .domain(d3.extent(data, d => new Date(d.x)));

const y = d3.scaleLinear()
    .range([height, 0])
    .domain([0, d3.max(data, d => d.y)]);

// Create axes
const xAxis = svg.append("g")
    .attr("transform", "translate(0," + height + ")")
    .call(d3.axisBottom(x).tickSize(0).tickValues([1, 3, 5, 7, 9])
        .tickFormat(function(d, i) {
            const dates = ["May 01", "May 15", "Jun 01", "Jun 15", "Jul 01", "Jul 15"];
            return dates[i];
        })
        .tickPadding(16) // Add padding between the ticks and the text
    )
    .call(g => g.select(".domain").style("stroke", "#EDEDED").style("stroke-width", "2px"))
    .selectAll("text")
    .style("text-anchor", "start"); // Align the text to the left

svg.append("g")
    .call(d3.axisLeft(y).tickSize(0).tickPadding(16)
        .tickValues([0, 0.5, 1, 1.5, 2]) // Specify the points at which ticks should be placed
        .tickFormat(function(d) { // Format the ticks
            if (d === 0) return "0";
            else if (d === 0.5) return "30 seg";
            else return d + " min";
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
    .attr("d", line); });