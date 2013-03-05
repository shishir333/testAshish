using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
namespace TestService
{
    class DBClass
    {
        FrameService.FramesDataWs frameService = new TestService.FrameService.FramesDataWs();

        string ticket;
        public string GetAuthorizationTicket()
        {
            DataSet ticketinfo = new DataSet();
            ticketinfo = frameService.FDWSAuthenticateForPMS("3586274A-B6D2-4EDC-8F69-9A669CE2B6CE", "KA1234567", "1", string.Empty, string.Empty);
            if (ticketinfo != null && ticketinfo.Tables.Count > 0 && ticketinfo.Tables[0].Rows.Count > 0)
            {
                ticket = Convert.ToString(ticketinfo.Tables[0].Rows[0][0]);
            }
            return ticket;
        }
        // Manufacture Information
        public void GetAllManufacture(string ticket, string connectionString)
        {
            DataSet dsManufacture = new DataSet();

            if (string.IsNullOrEmpty(ticket))
            {
                dsManufacture = frameService.FDWSGetManufacturers(ticket);
                if (dsManufacture != null && dsManufacture.Tables.Count > 0 && dsManufacture.Tables[0].Rows.Count > 0)
                {
                    // code to insert into the database
                    foreach (DataRow dr in dsManufacture.Tables[0].Rows)
                    {
                        //  int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertManufacture",Convert.ToString(dr[0]),Convert.ToString(dr[1]));
                        GetAllBrandByManufactureId(ticket, connectionString, Convert.ToString(dr[0]));
                    }
                }
            }
        }

        // Brand Information
        public void GetAllBrandByManufactureId(string ticket, string connectionString, string manufactureId)
        {
            DataSet dsBrand = new DataSet();

            if (string.IsNullOrEmpty(ticket))
            {
                dsBrand = frameService.FDWSGetBrandsByManufacturerID(ticket, manufactureId);
                if (dsBrand != null && dsBrand.Tables.Count > 0 && dsBrand.Tables[0].Rows.Count > 0)
                {
                    // code to insert into the database
                    foreach (DataRow dr in dsBrand.Tables[0].Rows)
                    {
                        // int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertBrand", Convert.ToString(dr[0]), Convert.ToString(dr[1]));
                        GetAllCollectionByBrandId(ticket, connectionString, Convert.ToString(dr[0]));
                    }
                }
            }
        }

        //collection Information
        public void GetAllCollectionByBrandId(string ticket, string connectionString, string brandId)
        {
            DataSet dsCollection = new DataSet();

            if (string.IsNullOrEmpty(ticket))
            {
                dsCollection = frameService.FDWSGetCollectionsByBrandID(ticket, brandId);
                if (dsCollection != null && dsCollection.Tables.Count > 0 && dsCollection.Tables[0].Rows.Count > 0)
                {
                    // code to insert into the database
                    foreach (DataRow dr in dsCollection.Tables[0].Rows)
                    {
                        // int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertCollection", Convert.ToString(dr[0]), Convert.ToString(dr[1]));
                        GetAllStyleByCollectionId(ticket, connectionString, Convert.ToString(dr[0]));
                    }
                }
            }
        }

        //collection Information
        public void GetAllStyleByCollectionId(string ticket, string connectionString, string collectionId)
        {
            DataSet dsStyle = new DataSet();

            if (string.IsNullOrEmpty(ticket))
            {
                dsStyle = frameService.FDWSGetStyleByCollectionID(ticket, collectionId);
                if (dsStyle != null && dsStyle.Tables.Count > 0 && dsStyle.Tables[0].Rows.Count > 0)
                {
                    // code to insert into the database
                    foreach (DataRow dr in dsStyle.Tables[0].Rows)
                    {
                        //   int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertCollection", Convert.ToString(dsStyle.Tables[0].Rows[0][0]), Convert.ToString(dsStyle.Tables[0].Rows[0][1]));
                        DataSet dsStyleInfo = new DataSet();
                        DataSet dsStyleConfig = new DataSet();
                        dsStyleInfo = frameService.FDWSGetStylePropertiesByStyleID(ticket, Convert.ToString(dr[0]));
                        dsStyleConfig = frameService.FDWSGetStyleConfigurationsByStyleID(ticket, Convert.ToString(dr[0]));
                        InsertStyleInfo(dsStyleInfo, dsStyleConfig, Convert.ToString(dr[0]), Convert.ToString(dr[1]), connectionString);
                    }
                }
            }
        }

        //insert style Information
        public void InsertStyleInfo(DataSet dsStyleInfo, DataSet dsStyleConfig, string styleId, string styleName,string connectionstring)
        {
            if (dsStyleInfo != null && dsStyleInfo.Tables.Count > 0 && dsStyleInfo.Tables[0].Rows.Count > 0)
            { 
                 string FrameManufacture = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][2]);
                 string  FrameBrand = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][3]);
                 string  FrameCollection = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][4]);
                 string  Status = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][6]);
                 DateTime StyleCreatedOn = System.DateTime.Now;
                 if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][7])))
                 {
                    StyleCreatedOn =Convert.ToDateTime(dsStyleInfo.Tables[0].Rows[0][7]);
                 }

                 DateTime StyleDiscontinuedOn = System.DateTime.Now;
                 if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][8])))
                 {
                    StyleDiscontinuedOn =Convert.ToDateTime(dsStyleInfo.Tables[0].Rows[0][8]);
                 }

                 DateTime StyleLastModifiedOn = System.DateTime.Now;
                 if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][9])))
                 {
                    StyleLastModifiedOn =Convert.ToDateTime(dsStyleInfo.Tables[0].Rows[0][9]);
                 }
                     
               
                 string  ProductGroup = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][10]);
                 string   Age = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][11]);
                 string   Gender = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][12]);
                 string   Material = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][13]);
                 string   MaterialDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][14]);
                 string  PreciousMetal = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][15]);
                 string  PreciousMetalDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][16]);
                 string   Country = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][17]);
                 string   TempleType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][18]);
                 string   TempleDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][19]);
                 string  BridgeType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][20]);
                 string   BridgeDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][21]);
                 string   Lens = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][22]);
                 string   LensDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][23]);
                 string   Trim = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][24]);
                 string   TrimDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][25]);
                 string   ClipSunglass = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][26]);
                 string   ClipsunglassDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][27]);
                 string   Sideshields = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][28]);
                 string   SideshieldsDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][29]);
                 string   EdgeType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][30]);
                 string   CaseType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][31]);
                 string    CaseDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][32]);
                 string   Hinge = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][33]);
                 string  RimType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][34]);
                 string  FrameShape = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][35]);
                 string  MonthIntro = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][36]);
                 string  YearIntro = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][37]);
                 string  Features = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][38]);
                 string   FramesPD = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][39]);
                 string   FramesPDDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][40]);
                 string   LensVision = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][41]);
                 string   LensVisionDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][42]);
                 string   RX = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][43]);
                 string   RXDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][44]);
                 string   Warranty = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][45]);
                 string   WarrantyDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][46]);
                 int   StyleNew =0;
                 if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][47])))
                 {
                    StyleNew =Convert.ToInt32(dsStyleInfo.Tables[0].Rows[0][47]);
                 }

                 int   BestSeller =0;
                 if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][48])))
                 {
                    BestSeller =Convert.ToInt32(dsStyleInfo.Tables[0].Rows[0][48]);
                 }
                
            
                string  ConfigurationFPC ;
                int   ConfigurationDefault=0 ;
                string   UPC ;
                string   SKU ;
                decimal  CompletePrice=0 ;
                decimal FrontPrice=0;
                decimal TemplesPrice=0;
                decimal HalfTemplesPrice=0;
                string   PriceDescription ;
                string  FrameColorGroup ;
                string  FrameColor ;
                string  LensColor ;
                string   LensColorCode ;
                string   EyeSize ;
                decimal   A =0;
                decimal   B =0;
                decimal   ED =0;
                decimal   EDAngle =0;
                decimal TempleLength=0;
                decimal BridgeSize=0;
                decimal DBL=0;
                decimal STS=0;
                decimal Circumference=0;
                decimal Ztilt=0;
                decimal FCRV=0;
                decimal FrameColorCode = 0;
                if (dsStyleConfig != null & dsStyleConfig.Tables.Count > 0 && dsStyleConfig.Tables[0].Rows.Count > 0)
                {
                    ConfigurationFPC = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][0]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][0])))
                    {
                        ConfigurationDefault = Convert.ToInt32(dsStyleConfig.Tables[0].Rows[0][1]);
                    }
                   
                    UPC = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][2]);
                    SKU =Convert.ToString(dsStyleConfig.Tables[0].Rows[0][3]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][4])))
                    {
                        CompletePrice = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][4]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][5])))
                    {
                        FrontPrice = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][5]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][6])))
                    {
                        TemplesPrice = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][6]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][7])))
                    {
                        HalfTemplesPrice = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][7]);
                    }

                     PriceDescription = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][8]);
                     FrameColorGroup = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][9]);
                     FrameColor = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][10]);
                     LensColor = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][11]);
                     LensColorCode = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][12]);
                     EyeSize = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][13]);
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][14])))
                     {
                         A = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][14]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][15])))
                     {
                         B = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][15]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][16])))
                     {
                         ED = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][16]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][17])))
                     {
                         EDAngle = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][17]);

                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][18])))
                     {
                         TempleLength = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][18]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][19])))
                     {
                         BridgeSize = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][19]);
                     }

                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][20])))
                     {
                         DBL = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][20]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][21])))
                     {
                         STS = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][21]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][22])))
                     {
                         Circumference = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][22]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][23])))
                     {
                         Ztilt = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][23]);

                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][24])))
                     {
                         FCRV = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][24]);
                     }
                     if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][25])))
                     {
                         FrameColorCode = Convert.ToDecimal(dsStyleConfig.Tables[0].Rows[0][25]);
                     }

                     int result = SqlHelper.ExecuteNonQuery(connectionstring, "JobSon_InsertStyle", styleId, styleName, FrameManufacture, FrameBrand,
                     FrameCollection, Status, StyleCreatedOn, StyleDiscontinuedOn, StyleLastModifiedOn, ProductGroup, Age, Gender, Material, MaterialDescription,
                     PreciousMetal, PreciousMetalDescription, Country, TempleType, TempleDescription, BridgeType, BridgeDescription, Lens, LensDescription,
                     Trim, TrimDescription, ClipSunglass, ClipsunglassDescription, Sideshields, SideshieldsDescription, EdgeType, CaseType, CaseDescription,
                     Hinge, RimType, FrameShape, MonthIntro, YearIntro, Features, FramesPD, FramesPDDescription, LensVision, LensVisionDescription,
                     RX, RXDescription, Warranty, WarrantyDescription, StyleNew, BestSeller, ConfigurationFPC, ConfigurationDefault, UPC, SKU, TemplesPrice,
                     HalfTemplesPrice, PriceDescription, FrameColorGroup, FrameColor, LensColor, LensColorCode, EyeSize, A, B, ED, EDAngle, TempleLength,
                     BridgeSize, DBL, STS, Circumference, Ztilt, FCRV, FrameColorCode);
                }

            }
        }
    }

}
