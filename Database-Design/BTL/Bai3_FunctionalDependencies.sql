-- Lam sach truoc khi tao lai bang R1 de tranh xung dot schema cu
DROP TABLE IF EXISTS r1 CASCADE;
CREATE TABLE r1 (
    a INT NOT NULL,  
    b INT NOT NULL,  
    c INT,           
    PRIMARY KEY (a, c)
);

-- Tuong tu cho bang R2: xoa neu ton tai va tao moi
DROP TABLE IF EXISTS r2 CASCADE;
CREATE TABLE r2 (
    a INT NOT NULL,   
    b INT NOT NULL, 
    c INT NOT NULL,
    d INT NOT NULL,
    e INT,
    PRIMARY KEY (a, b, e)
);

-- Function trigger kiem tra phu thuoc ham A -> B tren bang r1
CREATE OR REPLACE FUNCTION fd_r1_a_to_b()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    -- Kiem tra neu da co ban ghi cung gia tri a nhung b khac
    IF EXISTS (
        SELECT 1
        FROM r1
        WHERE a = NEW.a
          AND b IS DISTINCT FROM NEW.b
          AND (TG_OP = 'INSERT' OR (a, b, c) IS DISTINCT FROM (OLD.a, OLD.b, OLD.c))
    ) THEN
        RAISE EXCEPTION 'Vi pham phu thuoc ham A -> B tren R1: Gia tri a=% da ton tai voi b khac',
            NEW.a;
    END IF;
    RETURN NEW; -- Cho phep chen/cap nhat khi khong vi pham FD
END;
$$;

-- Trigger goi function tren moi khi INSERT/UPDATE r1
DROP TRIGGER IF EXISTS trg_fd_r1_a_to_b ON r1;
CREATE TRIGGER trg_fd_r1_a_to_b
BEFORE INSERT OR UPDATE ON r1
FOR EACH ROW
EXECUTE FUNCTION fd_r1_a_to_b();

-- Function trigger kiem tra phu thuoc ham AB -> CD tren bang r2
CREATE OR REPLACE FUNCTION fd_r2_ab_to_cd()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    -- Neu (a, b) da ton tai nhung (c, d) khac thi xem la vi pham
    IF EXISTS (
        SELECT 1
        FROM r2
        WHERE a = NEW.a
          AND b = NEW.b
          AND (c IS DISTINCT FROM NEW.c OR d IS DISTINCT FROM NEW.d)
          AND (TG_OP = 'INSERT' OR (a, b, c, d, e) IS DISTINCT FROM (OLD.a, OLD.b, OLD.c, OLD.d, OLD.e))
    ) THEN
        RAISE EXCEPTION 'Vi pham phu thuoc ham AB -> CD tren R2: Cap gia tri (a=%, b=%) da ton tai voi (c, d) khac',
            NEW.a, NEW.b;
    END IF;
    RETURN NEW; -- Chi cho phep thao tac hop le
END;
$$;

-- Trigger kich hoat check FD truoc moi thao tac INSERT/UPDATE tren r2
DROP TRIGGER IF EXISTS trg_fd_r2_ab_to_cd ON r2;
CREATE TRIGGER trg_fd_r2_ab_to_cd
BEFORE INSERT OR UPDATE ON r2
FOR EACH ROW
EXECUTE FUNCTION fd_r2_ab_to_cd();
