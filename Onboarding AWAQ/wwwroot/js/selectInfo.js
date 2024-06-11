    // Mapping between button text and card content
    const cardContents = {
        "Principios éticos": [
            {
                title: "Respeto",
                content: "Tratar a todos dignamente sin discriminación por sexo, raza, nacionalidad o religión."
            },
            {
                title: "Integridad",
                content: "Actuar con imparcialidad, justicia y buena fe, enfocado en pobreza y sostenibilidad."
            },
            {
                title: "Responsabilidad",
                content: "Respetar la ley, actuar honestamente y reconocer y solucionar errores."
            },
            {
                title: "Profesionalidad",
                content: "Compromiso con calidad y mejora continua mediante formación permanente."
            },
            {
                title: "Compromiso",
                content: "Mantener lealtad y compromiso con la organización y sus proyectos."
            },
            {
                title: "Participación",
                content: "Trabajar en equipo, compartir conocimientos y fomentar comunicación efectiva."
            },
            {
                title: "Transparencia",
                content: "Informar verazmente los resultados y responder a demandas de información."
            }
        ],
        "Conducta Básica": [
            {
                title: "ONGD Beneficio",
                content: "Evitar intereses personales y cualquier forma de discriminación."
            },
            {
                title: "Respeto Colaboradores",
                content: "Compromiso, igualdad, transparencia y buena fe con colaboradores."
            },
            {
                title: "Respeto Compañeros",
                content: "Respetar diferentes culturas, dialogar y trabajar en equipo."
            },
            {
                title: "Respeto ONG",
                content: "Actuar con eficacia, honradez, profesionalidad y responsabilidad."
            }
        ],
        "Normas Actuación": [
            {
                title: "Cumplir Leyes",
                content: "Todos deben cumplir las leyes de los países en que operan."
            },
            {
                title: "No Psicotrópicos",
                content: "Prohibido el uso, posesión o distribución de sustancias psicotrópicas."
            },
            {
                title: "Conflictos Interés",
                content: "Declarar cualquier conflicto de interés para su correcta resolución."
            },
            {
                title: "No Explotar",
                content: "Nunca explotar la vulnerabilidad de personas, especialmente mujeres o niños."
            },
            {
                title: "Confidencialidad",
                content: "Respetar la confidencialidad de la información y normas de ciberseguridad."
            },
            {
                title: "Regalos",
                content: "No aceptar ni ofrecer regalos que comprometan la objetividad del proyecto."
            },
            {
                title: "Dirección",
                content: "Garantizar conocimiento del código y tomar medidas disciplinarias."
            },
            {
                title: "Consecuencias",
                content: "El incumplimiento de la ley acarreará consecuencias legales."
            }
        ],
        "Gamificación TEDI": [
            {
                title: "HTML",
                content: "Identificar y manipular elementos HTML."
            },
            {
                title: "CSS",
                content: "Aplicar estilos CSS a elementos específicos."
            },
            {
                title: "TypeScript",
                content: "Manipulación de datos y diferencias con JavaScript."
            },
            {
                title: "React",
                content: "Creación y uso de props y state."
            },
            {
                title: "Tailwind",
                content: "Aplicar estilos con Tailwind CSS."
            },
            {
                title: "GitHub",
                content: "Gestión de proyectos y pull requests."
            },
            {
                title: "Scrum",
                content: "Planificación y ejecución de tareas ágiles."
            }
        ],
        "TEDI Estructura": [
            {
                title: "Carpetas",
                content: "Organización coherente y lógica de carpetas y recursos del proyecto."
            },
            {
                title: "Contenido",
                content: "Creación y gestión de contenido y traducciones."
            },
            {
                title: "Componentes",
                content: "Jerarquía de componentes y presentación en la interfaz."
            },
            {
                title: "Commits",
                content: "Entender commits, pushes y pull requests."
            }
        ],
        "Problemas Técnicos": [
            {
                title: "Código Correcto",
                content: "Identificar errores en HTML, Tailwind CSS, React y TypeScript."
            },
            {
                title: "Flujo Trabajo",
                content: "Evaluar la corrección del flujo de trabajo."
            }
        ],
        "Ciber Resiliencia": [
            {
                title: "Ataques",
                content: "Resistir y mitigar impactos de ataques cibernéticos."
            },
            {
                title: "Recuperación",
                content: "Restaurar funcionalidad rápidamente tras un incidente."
            },
            {
                title: "Incertidumbre",
                content: "Mantener la calma y adaptarse rápidamente a eventos imprevistos."
            },
            {
                title: "Aprendizaje",
                content: "Aprender de incidentes pasados y actualizar políticas."
            }
        ],
        "Seguridad Gamificación": [
            {
                title: "Educación",
                content: "Informar y concienciar sobre seguridad cibernética."
            },
            {
                title: "Evitar Ataques",
                content: "Capacitar para prevenir ataques."
            },
            {
                title: "Resiliencia",
                content: "Recuperarse rápidamente de incidentes."
            },
            {
                title: "Cumplimiento",
                content: "Cumplir normativas para proteger datos."
            }
        ],
        "Conceptos Clave": [
            {
                title: "Confidencialidad",
                content: "Mantener información privada y accesible solo para autorizados."
            },
            {
                title: "Ciberataques",
                content: "Ataques que comprometen datos."
            },
            {
                title: "Seguridad Móvil",
                content: "Proteger dispositivos móviles con contraseñas y cifrado."
            }
        ],
        "Contraseñas": [
            {
                title: "Longitud",
                content: "Contraseñas de al menos 12 caracteres, mezcla de caracteres."
            },
            {
                title: "Propiedad",
                content: "Contraseña exclusiva del usuario y no compartida."
            }
        ],
        "Phishing": [
            {
                title: "Phishing",
                content: "Ataque para robar datos mediante engaño."
            },
            {
                title: "Precauciones",
                content: "Estar alerta y tomar medidas preventivas."
            },
            {
                title: "Indicadores",
                content: `
                    Remitente: Verificar el dominio.
                    Mensaje: Buscar errores gramaticales.
                    Solicitudes: Revisar solicitudes inusuales.
                    Reporte: Reportar a seguridad@somosawaq.org.
                `
            }
        ],
        "Phishing Tips": [
            {
                title: "Correos Externos",
                content: `
                    No responder ni llamar, verificar el remitente.
                    Desconfiar de elogios y recompensas sin razón.
                    Verificar URL en inicio de sesión.
                `
            }
        ],
        "Cumplimiento": [
            {
                title: "Cero Tolerancia",
                content: "Política de cero tolerancia para fraudes o violaciones de seguridad."
            }
    ]
    };

    // Function to handle button click
    function handleButtonClick(event) {
        // Get all buttons
        const buttons = document.querySelectorAll('.info-select .btn');
        
        // Remove active class and add inactive class to all buttons
        buttons.forEach(button => {
            button.classList.remove('info-select-active');
            button.classList.add('info-select-inactive');
        });

        // Add active class to the clicked button
        event.currentTarget.classList.remove('info-select-inactive');
        event.currentTarget.classList.add('info-select-active');

        // Get the text content of the clicked button
        const buttonText = event.currentTarget.textContent.trim();

        // Get the corresponding card contents
        const cards = cardContents[buttonText];
        console.log(cards);

        // Clear existing cards
        const infoCards = document.querySelector('.info-cards');
        infoCards.innerHTML = '';
        console.log(infoCards);

        // Create and append new cards
        cards.forEach(cardContent => {
            const card = document.createElement('div');
            card.classList.add('info-card');

            // Set card title
            const title = document.createElement('h2');
            title.textContent = cardContent.title;
            card.appendChild(title);

            // Set card content
            const content = document.createElement('p');
            content.textContent = cardContent.content;
            card.appendChild(content);

            // Append new card
            infoCards.appendChild(card);
        });
    }

    // Attach click event listener to each button
    const buttons = document.querySelectorAll('.info-select .btn');
    buttons.forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });

    // Trigger click event on the first button to load its cards
    buttons[0].click();