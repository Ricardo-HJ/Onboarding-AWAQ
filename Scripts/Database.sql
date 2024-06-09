drop database if exists OnBoardingAWAQ;
create database OnBoardingAWAQ;

use OnBoardingAWAQ;

create table usuario(
	idUsuario int primary key auto_increment,
	nombre varchar(70),
	pais varchar(50),
	ciudad varchar(50),
	correo varchar(320),
	telefono varchar(15),
	contrasena varchar(150),
	superUsuario tinyint default 0,
    tiempoJugado int default 0,
    puntos int default 0,
    terminado varchar(20) default "En progresso",
    src varchar(150) default null
);

create table puntajeUsuario(
	idUsuario int,
    puntos int,
    fecha date,
	foreign key (idUsuario) references usuario(idUsuario)
);

create table departamento(
	idDepartamento int primary key auto_increment,
	departamento varchar(20)
);

create table usuarioDepartamento(
	idDepartamento int,
	idUsuario int,
	foreign key (idDepartamento) references departamento(idDepartamento),
	foreign key (idUsuario) references usuario(idUsuario)
);

create table token(
	idToken int primary key auto_increment,
	token varchar(6),
	idUsuario int,
	generado datetime,
	foreign key (idUsuario) references usuario(idUsuario)
);

create table zona(
	idZona int primary key auto_increment,
	zona varchar(20)
);

create table zonaUsuario(
	idZona int,
	idUsuario int,
	progreso int,
	foreign key (idUsuario) references usuario(idUsuario),
	foreign key (idZona) references zona(idZona)
);

create table minijuego(
	idMinijuego int primary key auto_increment,
	minijuego varchar(50),
	puntosBase int,
	idZona int,
	foreign key (idZona) references zona(idZona)
);

create table minijuegoUsuario(
	idMinijuego int,
    idUsuario int,
	segundos int default 0,
	completado tinyint,
    puntosObtenidos int,
	foreign key (idUsuario) references usuario(idUsuario),
	foreign key (idMinijuego) references minijuego(idMinijuego)
);

create table pregunta(
	idPregunta int primary key auto_increment,
	pregunta varchar(200),
	idMinijuego int,
	foreign key (idMinijuego) references minijuego(idMinijuego)
);

create table respuesta(
	idRespuesta int primary key auto_increment,
	respuesta varchar(100),
	idPregunta int,
	correcta tinyint,
	foreign key (idPregunta) references pregunta(idPregunta)
);

create table preguntaUsuario(
	idUsuario int,
    idPregunta int,
	segundos int default 0,
	acierto tinyint default 0,
	foreign key (idPregunta) references pregunta(idPregunta),
	foreign key (idUsuario) references usuario(idUsuario)
);

insert into usuario (nombre, pais, ciudad, correo, telefono, puntos, tiempoJugado, contrasena, superUsuario) values
	("Adrian", "Mexico", "Monterrey","atrevino136@gmail.com", "0451664", 190, 900, "$2a$11$4UvQq/bt1Ll6hkBEHOasguNs0Mi3vOH0yyrtOOnaqh4iFSMVOOeJO", 1),
    ("Ricardo", "Mexico", "Monterrey","rh9344001@gmail.com", "0451664", 190, 900, "$2a$11$hSMF5DE1WlavUO7939a1bO8sfYYPF0BaJypXaOkBweQ6spTu5E5RC", 1),
	("Pedro Jimenez", "Mexico", "Monterrey", "pedroJ@pedro.com", "1111111", 300,  1800, "$2a$11$4UvQq/bt1Ll6hkBEHOasguNs0Mi3vOH0yyrtOOnaqh4iFSMVOOeJO", 0),
	("Rodrigo Perez", "Colombia", "Medellin", "rodrigoP@rodrigo.com", "8888888", 25, 300, "RodrigoPRodrigo", 0),
	("Maria Rodriguez", "Colombia", "Medellin", "maria@gmail.com", "7777777", 30, 120, "ContraM123", 0),
	("Juan Rulfo", "España", "Barcelona", "Rulfo_Juan@yahoo.com", "6666666", 0, 0, "A143Vcew@19", 0);

insert into departamento (departamento) values 
	("TEDI"),
	("Recursos Humanos");

insert into usuarioDepartamento (idUsuario, idDepartamento) values
	(1,1),
	(2,2),
	(3,1),
	(4,1),
	(5,2);

insert into zona (zona) values
	("Rio - Ética"),
	("Bosque - Ética"),
	("Montaña - Estaciones"),
	("Ciudad - TEDI");

insert into zonaUsuario(idZona, idUsuario, progreso) values
	(1,1,50),
	(2,1,0),
	(3,1,0),
	(4,1,0),
	(1,2,100),
	(2,2,5),
	(3,2,0),
	(4,2,0);

insert into minijuego (minijuego, puntosBase, idZona) values	
	("Froghopper", 100, 1),
	("Petroleo", 100, 1),
	("CleaningRiver", 100, 1),
	("Berries", 100, 2),
	("Incendio", 100, 2),
	("Tala", 100, 2),
    ("Excavacion", 100, 4),
    ("Goat", 100, 4),
    ("Rocks", 100, 4),
	("Casa 1", 100, 3),
    ("Casa 2", 100, 3),
	("Casa 3", 100, 3);

insert into minijuegoUsuario (idMinijuego, idUsuario, segundos, completado, puntosObtenidos) values
	(1, 1, 15, 0, 30),
	(2, 1, 30, 1, 60),
    (4, 1, 10, 1, 100),
    (7, 1, 40, 1, 60),
    (10, 1, 30, 0, 0),
	(1, 2, 5, 0, 30),
	(2, 2, 60, 1, 95),
    (4, 2, 18, 1, 30),
    (7, 2, 12, 1, 5),
    (10, 2, 30, 0, 100);

insert into pregunta (pregunta, idMinijuego) VALUES
    ("¿Qué acción no está asociada con el principio de transparencia según el texto?", 1),
    ("¿Cuál es el primer principio ético mencionado en el código?", 4),
    ("¿Qué principio se refiere a actuar con imparcialidad, justicia y buena fe?", 5),
    ("¿Qué principio implica la dedicación y el compromiso con la calidad y la mejora continua?", 6),
    ("¿Qué principio se centra en trabajar en equipo y compartir conocimientos?", 7),
    ("¿Qué principio implica tratar a todas las personas dignamente, sin discriminación?", 8),
    ("¿Qué principio se refiere a acreditar de forma veraz y completa los resultados?", 9),
    ("¿Qué pauta básica implica actuar siempre en beneficio de la ONGD evitando intereses personales?", 10),
    ("¿Qué principio implica respetar la legislación y actuar honesta y ejemplarmente?", 11),
    ("¿Qué principio implica mantener la lealtad y compromiso con la organización?", 12),
    ("¿Qué principio implica colaborar e impulsar políticas inclusivas?", 1),
    ("¿Qué pauta implica actuar con compromiso, corresponsabilidad, igualdad e imparcialidad?", 2),
    ("¿Qué principio ético implica reconocer los errores, solucionándolos y evitando su repetición?", 3),
    ("¿Qué pauta implica respeto a las diferentes culturas, diálogo y trabajo en equipo?", 4),
    ("¿Qué principio ético implica la dedicación y el compromiso con la mejora continua?", 5),
    ("¿Qué pauta implica actuar con eficacia, honradez, profesionalidad y responsabilidad?", 6),
    ("¿Qué principio ético implica trabajar en equipo y compartir conocimientos?", 7),
    ("¿Qué pauta implica colaborar con transparencia y buena fe?", 8),
    ("¿Qué principio ético implica acreditar de forma veraz y completa los resultados?", 9),
    ("¿Qué principio implica mejorar conocimientos y capacidades mediante formación continua?", 10),
    ("¿Qué principio ético implica actuar con imparcialidad y justicia?", 11),
    ("¿Qué principio implica tratar a todas las personas dignamente?", 12),
    ("¿Qué pauta implica evitar intereses personales y cualquier discriminación?", 1),
    ("¿Qué principio implica actuar honesta y ejemplarmente, teniendo en cuenta las consecuencias en terceros?", 2),
    ("¿Qué principio ético implica colaborar de manera inclusiva y participativa?", 3),
    ("¿Qué pauta implica actuar con transparencia y buena fe con los colaboradores?", 4),
    ("¿Qué principio implica respetar la legislación y actuar con responsabilidad?", 5),
    ("¿Qué principio ético implica mejorar continuamente los conocimientos y capacidades?", 6),
    ("¿Qué pauta implica respetar las diferentes culturas y promover el diálogo?", 7),
    ("¿Qué principio ético implica actuar con imparcialidad y justicia?", 8),
    ("¿Qué principio implica tratar a todas las personas sin discriminación?", 9),
    ("¿Qué pauta implica actuar con compromiso, igualdad e imparcialidad?", 10),
    ("¿Qué principio ético implica la acreditación veraz de los resultados?", 11),
    ("¿Qué principio implica mejorar continuamente a través de la formación permanente?", 12),
    ("¿Qué pauta implica actuar siempre en beneficio de la ONGD?", 1),
    ("¿Qué principio ético implica actuar con imparcialidad y justicia?", 2),
    ("¿Qué principio implica tratar a todas las personas dignamente?", 3),
    ("¿Qué pauta implica colaborar con transparencia y buena fe?", 4),
    ("¿Qué principio ético implica mantener la lealtad con la organización?", 5),
    ("¿Qué pauta implica evitar intereses personales y cualquier discriminación?", 6),
    ("¿Qué principio ético implica actuar honesta y ejemplarmente?", 7),
    ("¿Qué principio implica trabajar en equipo y fomentar la participación?", 8),
    ("¿Qué pauta implica actuar con compromiso, igualdad y transparencia?", 9),
    ("¿Qué principio ético implica acreditar de forma veraz los resultados?", 10),
    ("¿Qué principio implica mejorar continuamente mediante la formación?", 11),
    ("¿Qué pauta implica respetar las diferentes culturas y promover el diálogo?", 12),
    ("¿Qué principio ético implica actuar con imparcialidad y justicia?", 1),
    ("¿Qué principio implica tratar a todas las personas sin discriminación?", 2),
    ("¿Qué pauta implica actuar con compromiso, igualdad y buena fe?", 3),

    -- Preguntas de Completar Frase
    ("El principio de ____ implica tratar a todas las personas dignamente, sin discriminación.", 4),
    ("El principio de ____ implica actuar con imparcialidad, justicia y buena fe.", 5),
    ("El principio de ____ implica acreditar de forma veraz y completa los resultados.", 6),
    ("El principio de ____ implica mejorar conocimientos y capacidades mediante formación continua.", 7),
    ("El principio de ____ implica mantener la lealtad y compromiso con la organización.", 8),

    -- Preguntas de Verdadero y Falso
    ("El principio de transparencia implica ocultar información para proteger la organización.", 9),
    ("Es aceptable aceptar regalos de proveedores si son de bajo valor y no influyen en decisiones de negocios.", 10),
    ("La responsabilidad incluye respetar la legislación y actuar de manera honesta y ejemplar.", 11),
    ("Es necesario reportar cualquier conflicto de interés a la organización.", 12),
    ("La profesionalidad implica solo cumplir con las tareas asignadas sin necesidad de mejorar continuamente.", 1);

insert into respuesta (respuesta, idPregunta, correcta) values 
	("Responder de forma diligente a las demandas de información",1, 0),
	("Aceptar críticas constructivas", 1, 0),
	("Ocultar información relevante", 1, 1),
	("Asumir responsabilidad por los resultados", 1, 0),
	("Respuesta 1",2, 0),
	("Respuesta 2",2, 0),
	("Respuesta 3",2, 1),
	("Respuesta 4",2, 0);

insert into preguntaUsuario(idUsuario, idPregunta, acierto, segundos) values
	(1,1,1,5),
    (1,2,1,10),
    (1,3,0,14),
    (1,4,0,20),
	(1,5,1,17),
    (1,6,1,12),
    (1,7,0,30),
    (1,8,0,1),
	(2,1,1,15),
    (2,2,0,30),
    (2,3,1,5),
    (2,4,1,2);

insert into puntajeUsuario(idUsuario, puntos, fecha) values
	(1, 0, "2024-05-23"),
    (1, 50, "2024-05-24"),
    (1, 120, "2024-05-25"),
    (1, 190, "2024-05-26"),
    (2, 75, "2024-05-23"),
	(2, 100, "2024-05-23"),
    (2, 180, "2024-05-24"),
    (2, 250, "2024-05-25"),
    (2, 300, "2024-05-26")
    ;

#### Store Procedures ###
## Dashboard Colaborador Procedures ##
/* Store Procedure getChangePuntaje */
Delimiter //
create procedure getPointsChange(idUsuario int)
	begin
		select pu.puntos, pu.fecha from puntajeUsuario pu where pu.idUsuario = idUsuario;
    end
//
Delimiter ;

/* Store Procedure getPuntajeZona */
Delimiter //
create procedure getZonePoints(idUsuario int)
	begin
		select
			z.zona,
			zu.progreso as "progreso",
            sum(mu.puntosObtenidos) as "puntos"
		from zonaUsuario zu
        inner join zona z on zu.idZona = z.idZona
        inner join minijuego m on m.idZona = z.idZona
        inner join minijuegoUsuario mu on mu.idMinijuego = m.idMinijuego and zu.idUsuario = mu.idUsuario
        where zu.idUsuario = idUsuario
        group by z.zona, zu.progreso;
    end
//
Delimiter ;

/* Store Procedure getTiempoPromedio */
Delimiter //
create procedure getAverageTime(idUsuario int)
	begin
		select avg(pu.segundos) as tiempo from preguntaUsuario pu where pu.idUsuario = idUsuario;
    end
//
Delimiter ;

/Store Procedure  getStatsPreguntas/alter
Delimiter //
create procedure getStatsPreguntas(idUsuario int)
	begin
		select
			"Correctas" as "Clase", count(pu.idPregunta) as "Cantidad"
			from preguntaUsuario pu 
			where pu.idUsuario = idUsuario and pu.acierto = 1
		union all
		select
			"Incorrectas", count(pu.idPregunta) 
			from preguntaUsuario pu 
			where pu.idUsuario = idUsuario and pu.acierto = 0
		union all
		select 
			"Pendientes", count(p.idPregunta) - (select count(pu.idPregunta) from preguntaUsuario pu where pu.idUsuario = idUsuario)
		from pregunta p;
	end
//
Delimiter ;

/* Tiempo promedio por area */
Delimiter //
create procedure getAreaStats(idUsuario int)
	begin
		select
			z.zona,
			zu.progreso as "progreso",
            sum(mu.puntosObtenidos) as "puntos",
            sum(mu.segundos) as "tiempo",
			sum(case pu.acierto when 1 then 1 else 0 end) as "pCorrectas",
			sum(case pu.acierto when 0 then 1 else 0 end) as "pIncorrectas"
		from zonaUsuario zu
        inner join zona z on zu.idZona = z.idZona
        inner join minijuego m on m.idZona = z.idZona
        inner join minijuegoUsuario mu on mu.idMinijuego = m.idMinijuego and zu.idUsuario = mu.idUsuario
        inner join pregunta p on p.idMinijuego = m.idMinijuego
        inner join preguntausuario pu on p.idPregunta = pu.idPregunta and pu.idUsuario = zu.idUsuario
        where zu.idUsuario = idUsuario
        group by z.zona, zu.progreso;
	end
//
Delimiter ;

## Usuario Procedures ##
/*Store Procedure getUserInfo */
Delimiter //
create procedure getUserInfoByCorreo(correo varchar(320))
	begin
		select u.idUsuario, u.contrasena, u.src, u.superUsuario from usuario u where u.correo = correo;
	end
//
Delimiter ;

/*Store Procedure getUsuario */
Delimiter //
create procedure getUsuarioByCorreo(correo varchar(320))
	begin
		select u.idUsuario, u.nombre from usuario u where u.correo = correo;
	end
//
Delimiter ;

/*Store Procedure createUsuario */
Delimiter //
create procedure createUsuario(nombre varchar(70), pais varchar(50), ciudad varchar(50), correo varchar(320), telefono varchar(15), contrasena varchar(150))
	begin
		insert into usuario (nombre, pais, ciudad, correo, telefono, contrasena) values (nombre, pais, ciudad, correo, telefono, contrasena);
	end
//
Delimiter ;
	
/Store Procedure changeSrc/
Delimiter //
create procedure changeSrc(src varchar(150), correo varchar(320))
	begin
			update usuario u set u.src = src where u.correo = correo;
	end
//
Delimiter ;

/*Store Procedure cambiar contraseña */
Delimiter //
create procedure cambiarContra(correo varchar(320), contrasena varchar(150))
	begin
		update usuario u set contrasena = contrasena where u.correo = correo;
	end
//
Delimiter ;

/* Store Procedure actualizar tiempo de juego */
Delimiter //
create procedure changePlayTime(idUsuario int, tiempoJugado varchar(150))
	begin
		update usuario u set u.tiempoJugado = tiempoJugado where u.idUsuario = idUsuario;
	end
//
Delimiter ;

/* Store Procedure leaderboard */
Delimiter //
create procedure getLeaderboard()
	begin
		select 
			u.src,
			u.nombre, 
			u.puntos,
			u.tiempoJugado,
			u.terminado,
			d.departamento
		from usuario u
		inner join usuarioDepartamento UD on UD.idUsuario = u.idUsuario
		inner join departamento d on d.idDepartamento = UD.idDepartamento
		order by puntos DESC;
	end
//
Delimiter ;

## Token Procedures ##	
/*Store Procedure insertToken */
Delimiter //
create procedure insertToken(token varchar(6), idUsuario int)
	begin
		insert into token (token, idUsuario, generado) values (token, idUsuario, now()); 
	end
//
Delimiter ;

/*Store Procedure getLastToken */
Delimiter //
create procedure getToken(idUsuario int)
	begin
		select t.token from token t where t.idUsuario = idUsuario order by generado desc limit 1;
	end
//
Delimiter ;

/*Store Procedure deleteToken */
Delimiter //
create procedure deleteToken(idUsuario int)
	begin
		delete from token t where t.idUsuario = idUsuario;
	end
//
Delimiter ;


## Departmento Procedures ##
Delimiter //
create procedure selectDepartmentID(departamento varchar(20))
	begin
		select d.idDepartamento from departamento d where d.departamento = departamento;
	end
//
Delimiter ;


## UsuarioDepartamento Procedures ##
/* Store Procedure create usuarioDepartamento */
Delimiter //
create procedure createUsuarioDepartamento(idDepartamento int, idUsuario int)
	begin
		insert into usuarioDepartamento (idDepartamento, idUsuario) values (idDepartamento, idUsuario);
	end
//
Delimiter ;
	

## Login Procedures ##
/* Store procedure LogIn */ 
Delimiter //
create procedure loginWeb(correo varchar(320))
	begin
		select u.idUsuario, u.contrasena, u.src, u.superUsuario from usuario u where u.correo = correo;
	end
//
Delimiter ;

/* Store Procedure LogIn VideoJuego */
Delimiter //
create procedure loginVideoJuego(correo varchar(320))
	begin
		select u.idUsuario, u.contrasena, d.departamento, u.tiempoJugado from usuario u
		inner join usuarioDepartamento ud on u.idUsuario = ud.idUsuario
		inner join departamento d on ud.idDepartamento = d.idDepartamento
		where u.correo = correo;
	end
//
Delimiter ;

## Preguntas Procedures ##
/* Store Procedure obtener preguntas minijuego */
Delimiter //
create procedure getPreguntasMinijuego(idMinijuego varchar(320))
	begin
		select 
			p.idPregunta, 
			p.pregunta
		from pregunta p 
		where p.idMinijuego = idMinijuego;
	end
//
Delimiter ;

Delimiter //
create procedure changePregunta(idPregunta int, idUsuario int, segundos int, acierto tinyint)
	begin
		update preguntaUsuario pu set 
			pu.segundos = segundos, 
			pu.acierto = acierto 
		where pu.idPregunta = idPregunta and pu.idUsuario = idUsuario;
	end
//
Delimiter ;
 

## Respuestas Procedures ##
/* Store Procedure obtener respuestas a preguntas */
Delimiter //
create procedure getRespuestasPregunta(idPregunta int)
	begin
		select 
			r.idRespuesta,
			r.respuesta,
			r.correcta
		from respuesta r where r.idPregunta = idPregunta;
	end
//
Delimiter ;
	
select * from usuario;

select * from token;

select * from departamento;

select * from usuarioDepartamento;

select * from zona;

select * from zonaUsuario;

select * from minijuegoUsuario;

select * from respuesta;

select * from pregunta;

select * from preguntausuario;