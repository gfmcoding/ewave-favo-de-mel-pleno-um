CREATE TABLE "order_tab" (
    id BIGINT NOT NULL PRIMARY KEY,
    table_number INT NOT NULL UNIQUE,
    is_closed BOOL NOT NULL
);

CREATE TABLE "order" (
    id BIGINT NOT NULL PRIMARY KEY,
    position INT NOT NULL UNIQUE,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(150) NOT NULL,
    status INT NOT NULL,
    order_tab_id BIGINT NOT NULL,
    FOREIGN KEY (order_tab_id) REFERENCES "order_tab" (id)
);