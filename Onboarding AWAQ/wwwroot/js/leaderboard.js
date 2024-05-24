// Define an array with the data for each row
const data = [
    { id: 1, name: "Jacob Jones", amount: 2500, duration: "2:35 Hr", date: "Miércoles 16:48", tag: "TEDI" },
    { id: 2, name: "Emily Smith", amount: 3000, duration: "1:45 Hr", date: "Jueves 09:30", tag: "XYZ" },
    { id: 3, name: "Michael Johnson", amount: 1800, duration: "3:10 Hr", date: "Viernes 14:15", tag: "ABC" },
    { id: 4, name: "Sophia Brown", amount: 3500, duration: "2:00 Hr", date: "Sábado 11:20", tag: "PQR" },
    { id: 5, name: "William Davis", amount: 2700, duration: "2:30 Hr", date: "Domingo 18:00", tag: "LMN" },
    { id: 6, name: "Olivia Wilson", amount: 2200, duration: "1:55 Hr", date: "Lunes 10:45", tag: "JKL" },
    { id: 7, name: "Ethan Martínez", amount: 3100, duration: "2:15 Hr", date: "Martes 13:25", tag: "GHI" },
    { id: 8, name: "Ava Anderson", amount: 2600, duration: "2:20 Hr", date: "Miércoles 20:10", tag: "DEF" },
    { id: 9, name: "Noah Thomas", amount: 2900, duration: "2:05 Hr", date: "Jueves 15:40", tag: "STU" },
    { id: 10, name: "Isabella García", amount: 2400, duration: "2:40 Hr", date: "Viernes 09:55", tag: "VWX" },
    { id: 11, name: "Michael Johnson", amount: 1800, duration: "3:10 Hr", date: "Viernes 14:15", tag: "ABC" },
    { id: 12, name: "Sophia Brown", amount: 3500, duration: "2:00 Hr", date: "Sábado 11:20", tag: "PQR" },
    { id: 13, name: "Michael Johnson", amount: 1800, duration: "3:10 Hr", date: "Viernes 14:15", tag: "ABC" }
];

  // Function to create a single lead row
function createLeadRow(data) {
    const row = document.createElement("div");
    row.classList.add("lead-row", "w-100");

    const idElement = document.createElement("p");
    idElement.textContent = data.id;
    idElement.style.width = "4rem";
    row.appendChild(idElement);

    const nameElement = document.createElement("p");
    nameElement.textContent = data.name;
    nameElement.style.width = "13rem";
    row.appendChild(nameElement);

    const amountElement = document.createElement("p");
    amountElement.textContent = data.amount;
    amountElement.style.width = "4rem";
    row.appendChild(amountElement);

    const durationElement = document.createElement("p");
    durationElement.textContent = data.duration;
    durationElement.style.width = "5rem";
    row.appendChild(durationElement);

    const dateElement = document.createElement("p");
    dateElement.textContent = data.date;
    dateElement.style.width = "7rem";
    row.appendChild(dateElement);

    const tagContainer = document.createElement("div");
    tagContainer.style.width = "12rem";

    const tagElement = document.createElement("div");
    tagElement.classList.add("colored-tag");

    const tagText = document.createElement("p");
    tagText.textContent = data.tag;
    tagElement.appendChild(tagText);

    tagContainer.appendChild(tagElement);
    row.appendChild(tagContainer);

    return row;
}

  // Get the container element where you want to append the rows
const container = document.getElementById("rows-container");

  // Loop through the data array and create lead rows
data.forEach((rowData) => {
    const row = createLeadRow(rowData);
    container.appendChild(row);
});


// Define an array with the data for each leaderboard card
const leaderboardData = [
    {
        position: 1,
        positionColor: "#F7C65E",
        name: "John Doe",
        tag: "TEDI",
        finishDate: "5 días",
        points: 1800,
        duration: "2:30 Hr",
        profileImage: "./img/profile.png"
    },
    {
        position: 2,
        positionColor: "#CE6930",
        name: "Jane Smith",
        tag: "ABC",
        finishDate: "3 días",
        points: 1600,
        duration: "1:45 Hr",
        profileImage: "./img/profile.png"
    },
    {
        position: 3,
        positionColor: "#651A19",
        name: "Michael Johnson",
        tag: "XYZ",
        finishDate: "7 días",
        points: 1400,
        duration: "2:00 Hr",
        profileImage: "./img/profile.png"
    }
];

  // Function to create a single leaderboard card
function createLeaderboardCard(data) {
    const card = document.createElement("div");
    card.classList.add("leaderboard-card");

    const firstColumn = document.createElement("div");
    firstColumn.classList.add("ld-cd-first");

    const position = document.createElement("h1");
    position.textContent = data.position;
    position.style.color = data.positionColor;
    firstColumn.appendChild(position);

    const infoContainer = document.createElement("div");
    infoContainer.classList.add("ld-cd-info");

    const profileImage = document.createElement("img");
    profileImage.src = data.profileImage;
    profileImage.alt = "profile";
    infoContainer.appendChild(profileImage);

    const nameContainer = document.createElement("div");
    nameContainer.classList.add("ld-cd-name");

    const nameText = document.createElement("p");
    nameText.textContent = data.name;
    nameContainer.appendChild(nameText);

    const tagElement = document.createElement("div");
    tagElement.classList.add("colored-tag");

    const tagText = document.createElement("p");
    tagText.textContent = data.tag;
    tagElement.appendChild(tagText);

    nameContainer.appendChild(tagElement);
    infoContainer.appendChild(nameContainer);
    firstColumn.appendChild(infoContainer);
    card.appendChild(firstColumn);

    const finishDateContainer = document.createElement("div");

    const finishDateText = document.createElement("p");
    finishDateText.classList.add("ld-cd-finish");
    finishDateText.textContent = `Terminó hace ${data.finishDate}`;
    finishDateContainer.appendChild(finishDateText);
    card.appendChild(finishDateContainer);

    const pointsContainer = document.createElement("div");
    pointsContainer.classList.add("ld-cd-points");

    const pointsText = document.createElement("h2");
    pointsText.textContent = `${data.points} pts`;
    pointsContainer.appendChild(pointsText);

    const durationText = document.createElement("h2");
    durationText.textContent = data.duration;
    pointsContainer.appendChild(durationText);
    card.appendChild(pointsContainer);

    return card;
}

  // Get the container element where you want to append the cards
const containerCard = document.getElementById("cards-container");

  // Loop through the leaderboardData array and create leaderboard cards
leaderboardData.forEach((cardData) => {
    const card = createLeaderboardCard(cardData);
    containerCard.appendChild(card);
});