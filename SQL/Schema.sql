drop function if exists update_final_price()
drop function if exists check_product_duplicate()

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
    stock int not null check (stock >= 0),
    image text default null
);

create table orders
(
    id serial not null primary key,
    user_id int not null references users (id) on delete cascade,
    order_date date not null,
    final_price numeric(13,2) not null default 0,
    active boolean not null default true
);

create table order_details
(
    order_id int not null references orders (id) on delete cascade,
    product_id int not null references products (id) on delete cascade,
    quantity int not null
);

/* FUNCTIONS */

create or replace function update_final_price()
returns trigger as
$$
begin
    if TG_OP = 'INSERT' then
        update orders
        set final_price = (
            select sum(quantity * products.price) as final_price
            from order_details
            join products on products.id = product_id
            where order_id = new.order_id
        )
        where id = new.order_id;
    else
        update orders
        set final_price = (
            select coalesce(sum(quantity * products.price), 0) as final_price
            from order_details
            join products on products.id = product_id
            where order_id = old.order_id
        )
        where id = old.order_id;
    end if;
    return new;
end;
$$
language plpgsql;

create or replace function check_product_duplicate()
returns trigger as
$$
begin
    if new.product_id not in (
        select product_id
        from order_details
        where new.order_id = order_id
    )
    then
        return new;
    else
        raise exception 'Product ID already exists with this order ID';
    end if;
end;
$$
language plpgsql;

/* TRIGGERS */

create trigger trg_update_final_price
after insert or update or delete on order_details
for each row execute function update_final_price();

create trigger trg_check_product_duplicate
before insert or update on order_details
for each row execute function check_product_duplicate();