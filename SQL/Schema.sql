drop trigger if exists trg_update_final_price on order_details;

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
    role varchar (32) not null default 'customer'
);

create table products
(
    id serial not null primary key,
    name varchar (64) unique not null,
    category varchar (64) not null,
    description text,
    price numeric(13,2) not null,
    stock int not null check (stock > 0),
    image text default null
);

create table orders
(
    id serial not null primary key,
    user_id int not null references users (id) on delete cascade,
    order_date date not null,
    final_price numeric(13,2) default 0,
    active boolean not null default true
);

create table order_details
(
    order_id int not null references orders (id) on delete cascade,
    product_id int not null references products (id) on delete cascade,
    quantity int not null
);

create or replace function update_final_price()
returns trigger as
$$
begin
    update orders
    set final_price = (
        select sum(quantity * products.price) as final_price
        from order_details
        join products on products.id = product_id
		where order_id = new.order_id
        group by order_id
    )
    where id = new.order_id;
    return new;
end;
$$
language plpgsql;

create trigger trg_update_final_price
after insert or update or delete
on order_details
for each row
execute function update_final_price();

if new.quantity > (select stock from products where id = new.product_id) then
		raise exception 'Out of stock (quantity is bigger than stock)';
    else

another trigger?
