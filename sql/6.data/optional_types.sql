INSERT INTO ObjectType( ID, Name ) VALUES( 1, 'Asset' );
INSERT INTO FormatType( ID, Name )VALUES( 1, 'Video' );
INSERT INTO FormatType( ID, Name )VALUES( 2, 'Audio' );
INSERT INTO FormatType( ID, Name )VALUES( 3, 'Image' );
INSERT INTO FormatType( ID, Name )VALUES( 4, 'Other' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 1, 1, 'Video Source' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 2, 2, 'Audio Source' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 3, 3, 'Image Source' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 4, 4, 'Other Source' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 5, 1, 'Video Preview' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 6, 2, 'Audio Preview' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 7, 3, 'Image Preview' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 8, 4, 'Other Preview' );
INSERT INTO FormatCategory( ID, FormatTypeID, Name )VALUES( 9, 3, 'Thumbnail' );
INSERT INTO Format( ID, FormatCategoryID, Name, FormatXML, MimeType )VALUES( 1, 1, 'Unknown Video', null, 'application/octet-stream');
INSERT INTO Format( ID, FormatCategoryID, Name, FormatXML, MimeType )VALUES( 2, 2, 'Unknown Audio', null, 'application/octet-stream');
INSERT INTO Format( ID, FormatCategoryID, Name, FormatXML, MimeType )VALUES( 3, 3, 'Unknown Image', null, 'application/octet-stream');
INSERT INTO Format( ID, FormatCategoryID, Name, FormatXML, MimeType )VALUES( 4, 4, 'Unknown Other', null, 'application/octet-stream');
INSERT INTO Format( ID, FormatCategoryID, Name, FormatXML, MimeType,Extension )VALUES( 5, 9, 'PNG', null, 'image/png', '.png');
INSERT INTO ObjectRelationType( ID, Name ) VALUES( 1, 'Contains' );
INSERT INTO ObjectRelationType( ID, Name ) VALUES( 2, 'Part of' );

