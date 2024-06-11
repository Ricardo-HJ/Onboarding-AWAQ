fetch(`https://localhost:7117/api/Web/getAverageDepartamento`, {
    method: 'GET',
    mode: "cors"
})
.then(response => response.json())
.then(results => {
    const departmentData = [];
    for (var i = 0; i < results.length; i++) {
        setData(results[i], departmentData);
    }
    createGraph(departmentData);
})
.catch(e => {
    console.log("Error" + e);
})

function setData(departmentInfo, departmentData) {
    var department = departmentInfo["departamento"];
    var completion = departmentInfo["progreso"];
    departmentData.push({ x: department, y: completion });
}

function createGraph(dataDepartment) {
    // Set up the dimensions of the chart
    var margin = { top: 20, right: 0, bottom: 30, left: 0 },
        width = 356,
        height = 147 - margin.top - margin.bottom;

    var data = [
        { x: dataDepartment[0]["x"], y: dataDepartment[0]["y"], color: '#111111' },
        { x: dataDepartment[1]["x"], y: dataDepartment[1]["y"], color: '#2d572a' }
    ];

    // Create the SVG container
    const svg = d3.selectAll("#barsgraph").each(function () {
        var svg = d3.select(this).append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
            .append("g")
            .attr("transform", `translate(${margin.left}, ${margin.top})`);

        // Define the scales for x and y axes
        const y = d3.scaleBand()
            .range([0, height])
            .domain(data.map(d => d.x))
            .padding(0.1);

        const x = d3.scaleLinear()
            .range([0, width])
            .domain([0, d3.max(data, d => d.y)]);

        // Remove all ticks
        svg.selectAll(".tick line").remove();

        // Style the text
        d3.selectAll("text")
            .style("fill", "white")
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
            .attr("y", d => y(d.x))
            .attr("height", y.bandwidth())
            .attr("x", 0)
            .attr("width", d => x(d.y))
            .attr("fill", d => d.color)
            .attr("rx", 14) // Set the radius for rounded corners
            .attr("ry", 14); // Set the radius for rounded corners

        // Add text labels to the bars
        svg.selectAll(".label")
            .data(data)
            .enter().append("text")
            .attr("class", "label")
            .attr("y", d => y(d.x) + y.bandwidth() / 2 + 5) // Centered vertically
            .attr("x", d => x(d.y) / 2) // Centered horizontally
            .attr("text-anchor", "middle")
            .style("fill", "white")
            .style("font-family", "Inter")
            .style("font-size", "12px")
            .style("font-weight", "400")
            .text(d => d.y + "%");
    });
}