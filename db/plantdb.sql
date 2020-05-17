CREATE TABLE plant (
    PlantID INT PRIMARY KEY NOT NULL,
    CommonName VARCHAR(128),
    ScientificName VARCHAR(128),
    PlantDescription VARCHAR(16384),
    IsEdible TINYINT NOT NULL
);

CREATE TABLE attribute (
    AttributeID INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    PlantID INT NOT NULL,
    AttributeDescription VARCHAR(16384),
    CONSTRAINT FOREIGN KEY (PlantID) REFERENCES
        plant (PlantID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE image (
    ImageID INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    PlantID INT NOT NULL,
    LinkToFile VARCHAR(512),
    CONSTRAINT FOREIGN KEY (PlantID) REFERENCES
        plant (PlantID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE region_shape_file (
    RegionShapeFileID INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    PlantID INT NOT NULL,
    LinkToFile VARCHAR(512),
    CONSTRAINT FOREIGN KEY (PlantID) REFERENCES
        plant (PlantID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);
