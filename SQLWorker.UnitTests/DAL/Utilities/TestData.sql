create table if not exists usertable
(
	id serial not null
		constraint usertable_pkey
			primary key,
	username varchar(8) not null,
	email varchar(50) not null
);

alter table usertable owner to postgres;

create unique index if not exists usertable_id_uindex
	on usertable (id);

create unique index if not exists usertable_email_uindex
	on usertable (email);

create unique index if not exists usertable_username_uindex
	on usertable (username);

INSERT INTO public.usertable (id, username, email) VALUES (1, 'ya', 'b@gmail.com');
INSERT INTO public.usertable (id, username, email) VALUES (2, 'ne', 'a@gmail.com');

create table if not exists users
(
	id bigserial not null
		constraint users_pk
			primary key,
	email varchar(50) not null,
	name varchar(25) not null,
	password varchar(50) not null
);

alter table users owner to postgres;

create unique index if not exists users_email_uindex
	on users (email);

create unique index if not exists users_id_uindex
	on users (id);

INSERT INTO public.users (id, email, name, password) VALUES (8, 'stas@gmail.com', 'Stanislav', '黧蹁扈椵廗笪급瞛⚱䖤ﾹ욝骘禠');