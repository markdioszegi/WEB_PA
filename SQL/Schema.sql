drop table if exists order_details;
drop table if exists orders;
drop table if exists products;
drop table if exists users;

create table users
(
    id serial not null primary key,
    username varchar (32) unique not null,
    password varchar (32) not null,
    email varchar (64) not null,
    role varchar (32) not null
);

create table products
(
    id serial not null primary key,
    category varchar (64) not null,
    name varchar (64) not null,
    description text,
    price numeric(13,2) not null,
    quantity int not null check (quantity > 0),
    image text default null
);

create table orders
(
    id serial not null primary key,
    user_id int not null references users (id) on delete cascade,
    order_date date not null,
    final_price numeric(13,2) not null,
    status boolean not null default false
);

create table order_details
(
    order_id int not null references orders (id) on delete cascade,
    product_id int not null references products (id) on delete cascade
);

create or replace function update_final_price
()
returns trigger as
$$
begin
    if new.quantity < 0 then
    raise exception 'Quantity cannot be less than 0';
end
if;
    return new;
end;
$$ LANGUAGE plpgsql;

create trigger trg_update_final_price before
insert or
update on order_details
for each row
execute function update_final_price
();
