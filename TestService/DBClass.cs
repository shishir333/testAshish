using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.IO;
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
            try
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    dsManufacture = frameService.FDWSGetManufacturers(ticket);
                    if (dsManufacture != null && dsManufacture.Tables.Count > 0 && dsManufacture.Tables[0].Rows.Count > 0)
                    {
                        // code to insert into the database
                        foreach (DataRow dr in dsManufacture.Tables[0].Rows)
                        {
                            int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertManufacture", Convert.ToString(dr[1]), Convert.ToString(dr[0]));
                            GetAllBrandByManufactureId(ticket, connectionString, Convert.ToString(dr[0]));
                        }
                    }
                }
            }
            catch
            {

            }
        }

        // Brand Information
        public void GetAllBrandByManufactureId(string ticket, string connectionString, string manufactureId)
        {
            DataSet dsBrand = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    dsBrand = frameService.FDWSGetBrandsByManufacturerID(ticket, manufactureId);
                    if (dsBrand != null && dsBrand.Tables.Count > 0 && dsBrand.Tables[0].Rows.Count > 0)
                    {
                        // code to insert into the database
                        foreach (DataRow dr in dsBrand.Tables[0].Rows)
                        {
                            int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertBrand", Convert.ToString(dr[1]), Convert.ToString(dr[0]));
                            GetAllCollectionByBrandId(ticket, connectionString, Convert.ToString(dr[0]), manufactureId);
                        }
                    }
                }
            }
            catch
            { }
        }

        //collection Information
        public void GetAllCollectionByBrandId(string ticket, string connectionString, string brandId, string manfuId)
        {
            DataSet dsCollection = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    dsCollection = frameService.FDWSGetCollectionsByBrandID(ticket, brandId);
                    if (dsCollection != null && dsCollection.Tables.Count > 0 && dsCollection.Tables[0].Rows.Count > 0)
                    {
                        // code to insert into the database
                        foreach (DataRow dr in dsCollection.Tables[0].Rows)
                        {
                            int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertCollection", Convert.ToString(dr[1]), Convert.ToString(dr[0]));
                            GetAllStyleByCollectionId(ticket, connectionString, Convert.ToString(dr[0]), brandId, manfuId);
                        }
                    }
                }
            }
            catch
            { }
        }

        //collection Information
        public void GetAllStyleByCollectionId(string ticket, string connectionString, string collectionId, string brandId, string manfuId)
        {
            DataSet dsStyle = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    dsStyle = frameService.FDWSGetStyleByCollectionID(ticket, collectionId);
                    if (dsStyle != null && dsStyle.Tables.Count > 0 && dsStyle.Tables[0].Rows.Count > 0)
                    {
                        // code to insert into the database
                        foreach (DataRow dr in dsStyle.Tables[0].Rows)
                        {
                            int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertCollection", Convert.ToString(dsStyle.Tables[0].Rows[0][1]), Convert.ToString(dsStyle.Tables[0].Rows[0][0]));
                            DataSet dsStyleInfo = new DataSet();
                            DataSet dsStyleConfig = new DataSet();
                            dsStyleInfo = frameService.FDWSGetStylePropertiesByStyleID(ticket, Convert.ToString(dr[0]));
                            dsStyleConfig = frameService.FDWSGetStyleConfigurationsByStyleID(ticket, Convert.ToString(dr[0]));
                            InsertStyleInfo(dsStyleInfo, dsStyleConfig, Convert.ToString(dr[0]), Convert.ToString(dr[1]), connectionString, brandId, manfuId, collectionId);
                        }
                    }
                }
            }
            catch
            { }
        }

        //insert style Information
        public void InsertStyleInfo(DataSet dsStyleInfo, DataSet dsStyleConfig, string styleId, string styleName, string connectionstring, string brandid, string manfuid, string collectionid)
        {
            try
            {
                if (dsStyleInfo != null && dsStyleInfo.Tables.Count > 0 && dsStyleInfo.Tables[0].Rows.Count > 0)
                {
                    string FrameManufacture = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][2]);
                    string FrameBrand = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][3]);
                    string FrameCollection = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][4]);
                    string Status = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][6]);
                   
                    string StyleCreatedOn = null;
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][7]).Trim()))
                    {
                        StyleCreatedOn = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][7]);
                    }

                    string StyleDiscontinuedOn = null;
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][8]).Trim()))
                    {
                        StyleDiscontinuedOn = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][8]);
                    }

                    string StyleLastModifiedOn = null;
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][9]).Trim()))
                    {
                        StyleLastModifiedOn = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][9]);
                    }


                    string ProductGroup = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][10]);
                    string Age = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][11]);
                    string Gender = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][12]);
                    string Material = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][13]);
                    string MaterialDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][14]);
                    string PreciousMetal = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][15]);
                    string PreciousMetalDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][16]);
                    string Country = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][17]);
                    string TempleType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][18]);
                    string TempleDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][19]);
                    string BridgeType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][20]);
                    string BridgeDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][21]);
                    string Lens = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][22]);
                    string LensDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][23]);
                    string Trim = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][24]);
                    string TrimDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][25]);
                    string ClipSunglass = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][26]);
                    string ClipsunglassDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][27]);
                    string Sideshields = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][28]);
                    string SideshieldsDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][29]);
                    string EdgeType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][30]);
                    string CaseType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][31]);
                    string CaseDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][32]);
                    string Hinge = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][33]);
                    string RimType = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][34]);
                    string FrameShape = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][35]);
                    string MonthIntro = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][36]);
                    string YearIntro = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][37]);
                    string Features = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][38]);
                    string FramesPD = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][39]);
                    string FramesPDDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][40]);
                    string LensVision = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][41]);
                    string LensVisionDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][42]);
                    string RX = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][43]);
                    string RXDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][44]);
                    string Warranty = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][45]);
                    string WarrantyDescription = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][46]);
                    string StyleNew = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][47]).Trim()))
                    {
                        StyleNew = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][47]);
                    }

                    string BestSeller = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(dsStyleInfo.Tables[0].Rows[0][48]).Trim()))
                    {
                        BestSeller = Convert.ToString(dsStyleInfo.Tables[0].Rows[0][48]);
                    }


                    string ConfigurationFPC;
                    string ConfigurationDefault = string.Empty;
                    string UPC;
                    string SKU;
                    string CompletePrice = string.Empty;
                    string FrontPrice = string.Empty;
                    string TemplesPrice = string.Empty;
                    string HalfTemplesPrice = string.Empty;
                    string PriceDescription;
                    string FrameColorGroup;
                    string FrameColor;
                    string LensColor;
                    string LensColorCode;
                    string EyeSize;
                    string A = string.Empty;
                    string B = string.Empty;
                    string ED = string.Empty;
                    string EDAngle = string.Empty;
                    string TempleLength = string.Empty;
                    string BridgeSize = string.Empty;
                    string DBL = string.Empty;
                    string STS = string.Empty;
                    string Circumference = string.Empty;
                    string Ztilt = string.Empty;
                    string FCRV = string.Empty;
                    string FrameColorCode = string.Empty;
                    if (dsStyleConfig != null & dsStyleConfig.Tables.Count > 0 && dsStyleConfig.Tables[0].Rows.Count > 0)
                    {
                        ConfigurationFPC = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][0]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][1]).Trim()))
                        {
                            ConfigurationDefault = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][1]);
                        }

                        UPC = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][2]);
                        SKU = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][3]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][4]).Trim()))
                        {
                            CompletePrice = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][4]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][5]).Trim()))
                        {
                            FrontPrice = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][5]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][6]).Trim()))
                        {
                            TemplesPrice = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][6]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][7]).Trim()))
                        {
                            HalfTemplesPrice = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][7]);
                        }

                        PriceDescription = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][8]);
                        FrameColorGroup = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][9]);
                        FrameColor = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][10]);
                        LensColor = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][11]);
                        LensColorCode = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][12]);
                        EyeSize = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][13]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][14]).Trim()))
                        {
                            A = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][14]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][15]).Trim()))
                        {
                            B = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][15]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][16]).Trim()))
                        {
                            ED = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][16]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][17]).Trim()))
                        {
                            EDAngle = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][17]);

                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][18]).Trim()))
                        {
                            TempleLength = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][18]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][19]).Trim()))
                        {
                            BridgeSize = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][19]);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][20]).Trim()))
                        {
                            DBL = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][20]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][21]).Trim()))
                        {
                            STS = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][21]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][22]).Trim()))
                        {
                            Circumference = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][22]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][23]).Trim()))
                        {
                            Ztilt = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][23]);

                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][24]).Trim()))
                        {
                            FCRV = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][24]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dsStyleConfig.Tables[0].Rows[0][25]).Trim()))
                        {
                            FrameColorCode = Convert.ToString(dsStyleConfig.Tables[0].Rows[0][25]);
                        }

                        int result = SqlHelper.ExecuteNonQuery(connectionstring, "JobSon_InsertStyle", manfuid, brandid, collectionid, styleId, styleName, FrameManufacture, FrameBrand,
                        FrameCollection, Status, StyleCreatedOn, StyleDiscontinuedOn, StyleLastModifiedOn, ProductGroup, Age, Gender, Material, MaterialDescription,
                        PreciousMetal, PreciousMetalDescription, Country, TempleType, TempleDescription, BridgeType, BridgeDescription, Lens, LensDescription,
                        Trim, TrimDescription, ClipSunglass, ClipsunglassDescription, Sideshields, SideshieldsDescription, EdgeType, CaseType, CaseDescription,
                        Hinge, RimType, FrameShape, MonthIntro, YearIntro, Features, FramesPD, FramesPDDescription, LensVision, LensVisionDescription,
                        RX, RXDescription, Warranty, WarrantyDescription, StyleNew, BestSeller, ConfigurationFPC, ConfigurationDefault, UPC, SKU, CompletePrice, FrontPrice,
                        TemplesPrice, HalfTemplesPrice, PriceDescription, FrameColorGroup, FrameColor, LensColor, LensColorCode, EyeSize,
                        A, B, ED, EDAngle, TempleLength, BridgeSize, DBL, STS, Circumference, Ztilt, FCRV, FrameColorCode);
                    }

                }
            }
            catch
            { }
        }

        //Insert Style From File Information
        public void InsertStyleFromFile(string connectionString)
        {
            try
            {
                using (TextReader tr = File.OpenText(@"C:\Users\hnguyen\Downloads\WebserviceData_Updated.txt"))
                {
                    string strLine = string.Empty;
                    string[] arrColumns = null;
                    int count = 0;
                    while ((strLine = tr.ReadLine()) != null)
                    {

                        arrColumns = strLine.Split('\t');
                        if (count > 0)
                        {
                            // Start Fill Your DataSet or Whatever you wanna do with your data
                            string FrameManufacture = Convert.ToString(arrColumns[4]);
                            string FrameBrand = Convert.ToString(arrColumns[5]);
                            string FrameCollection = Convert.ToString(arrColumns[6]);
                            string Status = Convert.ToString(arrColumns[8]);
                            string StyleCreatedOn = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[9]).Trim()))
                            {
                                StyleCreatedOn = Convert.ToString(arrColumns[9]);
                            }

                            string StyleDiscontinuedOn = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[10]).Trim()))
                            {
                                StyleDiscontinuedOn = Convert.ToString(arrColumns[10]);
                            }

                            string StyleLastModifiedOn = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[11]).Trim()))
                            {
                                StyleLastModifiedOn = Convert.ToString(arrColumns[11]);
                            }


                            string ProductGroup = Convert.ToString(arrColumns[12]);
                            string Age = Convert.ToString(arrColumns[13]);
                            string Gender = Convert.ToString(arrColumns[14]);
                            string Material = Convert.ToString(arrColumns[15]);
                            string MaterialDescription = Convert.ToString(arrColumns[16]);
                            string PreciousMetal = Convert.ToString(arrColumns[17]);
                            string PreciousMetalDescription = Convert.ToString(arrColumns[18]);
                            string Country = Convert.ToString(arrColumns[19]);
                            string TempleType = Convert.ToString(arrColumns[20]);
                            string TempleDescription = Convert.ToString(arrColumns[21]);
                            string BridgeType = Convert.ToString(arrColumns[22]);
                            string BridgeDescription = Convert.ToString(arrColumns[23]);
                            string Lens = Convert.ToString(arrColumns[24]);
                            string LensDescription = Convert.ToString(arrColumns[25]);
                            string Trim = Convert.ToString(arrColumns[26]);
                            string TrimDescription = Convert.ToString(arrColumns[27]);
                            string ClipSunglass = Convert.ToString(arrColumns[28]);
                            string ClipsunglassDescription = Convert.ToString(arrColumns[29]);
                            string Sideshields = Convert.ToString(arrColumns[30]);
                            string SideshieldsDescription = Convert.ToString(arrColumns[31]);
                            string EdgeType = Convert.ToString(arrColumns[32]);
                            string CaseType = Convert.ToString(arrColumns[33]);
                            string CaseDescription = Convert.ToString(arrColumns[34]);
                            string Hinge = Convert.ToString(arrColumns[35]);
                            string RimType = Convert.ToString(arrColumns[36]);
                            string FrameShape = Convert.ToString(arrColumns[37]);
                            string MonthIntro = Convert.ToString(arrColumns[38]);
                            string YearIntro = Convert.ToString(arrColumns[39]);
                            string Features = Convert.ToString(arrColumns[40]);
                            string FramesPD = Convert.ToString(arrColumns[41]);
                            string FramesPDDescription = Convert.ToString(arrColumns[42]);
                            string LensVision = Convert.ToString(arrColumns[43]);
                            string LensVisionDescription = Convert.ToString(arrColumns[44]);
                            string RX = Convert.ToString(arrColumns[45]);
                            string RXDescription = Convert.ToString(arrColumns[46]);
                            string Warranty = Convert.ToString(arrColumns[47]);
                            string WarrantyDescription = Convert.ToString(arrColumns[48]);
                            string StyleNew = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[49]).Trim()))
                            {
                                StyleNew = Convert.ToString(arrColumns[49]);
                            }

                            string BestSeller = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[50]).Trim()))
                            {
                                BestSeller = Convert.ToString(arrColumns[50]);
                            }


                            string ConfigurationFPC;
                            string ConfigurationDefault = string.Empty;
                            string UPC;
                            string SKU;
                            string CompletePrice = string.Empty;
                            string FrontPrice = string.Empty;
                            string TemplesPrice = string.Empty;
                            string HalfTemplesPrice = string.Empty;
                            string PriceDescription;
                            string FrameColorGroup;
                            string FrameColor;
                            string LensColor;
                            string LensColorCode;
                            string EyeSize;
                            string A = string.Empty;
                            string B = string.Empty;
                            string ED = string.Empty;
                            string EDAngle = string.Empty;
                            string TempleLength = string.Empty;
                            string BridgeSize = string.Empty;
                            string DBL = string.Empty;
                            string STS = string.Empty;
                            string Circumference = string.Empty;
                            string Ztilt = string.Empty;
                            string FCRV = string.Empty;
                            string FrameColorCode = string.Empty;
                            string ConfigurationImageName = string.Empty;
                            // if (dsStyleConfig != null & dsStyleConfig.Tables.Count > 0 && dsStyleConfig.Tables[0].Rows.Count > 0)
                            {
                                ConfigurationFPC = Convert.ToString(arrColumns[51]);
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[52]).Trim()))
                                {
                                    ConfigurationDefault = Convert.ToString(arrColumns[52]);
                                }

                                UPC = Convert.ToString(arrColumns[53]);
                                SKU = Convert.ToString(arrColumns[54]);
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[55]).Trim()))
                                {
                                    CompletePrice = Convert.ToString(arrColumns[55]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[56]).Trim()))
                                {
                                    FrontPrice = Convert.ToString(arrColumns[56]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[57]).Trim()))
                                {
                                    TemplesPrice = Convert.ToString(arrColumns[57]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[58]).Trim()))
                                {
                                    HalfTemplesPrice = Convert.ToString(arrColumns[58]);
                                }

                                PriceDescription = Convert.ToString(arrColumns[59]);
                                FrameColorGroup = Convert.ToString(arrColumns[60]);
                                FrameColor = Convert.ToString(arrColumns[61]);
                                LensColor = Convert.ToString(arrColumns[62]);
                                LensColorCode = Convert.ToString(arrColumns[63]);
                                EyeSize = Convert.ToString(arrColumns[64]);
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[65]).Trim()))
                                {
                                    A = Convert.ToString(arrColumns[65]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[66]).Trim()))
                                {
                                    B = Convert.ToString(arrColumns[66]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[67]).Trim()))
                                {
                                    ED = Convert.ToString(arrColumns[67]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[68]).Trim()))
                                {
                                    EDAngle = Convert.ToString(arrColumns[68]);

                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[69]).Trim()))
                                {
                                    TempleLength = Convert.ToString(arrColumns[69]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[70]).Trim()))
                                {
                                    BridgeSize = Convert.ToString(arrColumns[70]);
                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[71]).Trim()))
                                {
                                    DBL = Convert.ToString(arrColumns[71]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[72]).Trim()))
                                {
                                    STS = Convert.ToString(arrColumns[72]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[73]).Trim()))
                                {
                                    Circumference = Convert.ToString(arrColumns[73]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[74]).Trim()))
                                {
                                    Ztilt = Convert.ToString(arrColumns[74]);

                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[75]).Trim()))
                                {
                                    FCRV = Convert.ToString(arrColumns[75]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[76]).Trim()))
                                {
                                    FrameColorCode = Convert.ToString(arrColumns[76]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(arrColumns[77]).Trim()))
                                {
                                    ConfigurationImageName = Convert.ToString(arrColumns[77]);
                                }
                                int result = SqlHelper.ExecuteNonQuery(connectionString, "JobSon_InsertStyle", Convert.ToString(arrColumns[0]),
                                Convert.ToString(arrColumns[2]), Convert.ToString(arrColumns[3]), Convert.ToString(arrColumns[1]),
                                Convert.ToString(arrColumns[7]), FrameManufacture, FrameBrand,
                                FrameCollection, Status, StyleCreatedOn, StyleDiscontinuedOn, StyleLastModifiedOn, ProductGroup, Age, Gender, Material, MaterialDescription,
                                PreciousMetal, PreciousMetalDescription, Country, TempleType, TempleDescription, BridgeType, BridgeDescription, Lens, LensDescription,
                                Trim, TrimDescription, ClipSunglass, ClipsunglassDescription, Sideshields, SideshieldsDescription, EdgeType, CaseType, CaseDescription,
                                Hinge, RimType, FrameShape, MonthIntro, YearIntro, Features, FramesPD, FramesPDDescription, LensVision, LensVisionDescription,
                                RX, RXDescription, Warranty, WarrantyDescription, StyleNew, BestSeller, ConfigurationFPC, ConfigurationDefault, UPC, SKU, CompletePrice, FrontPrice,
                                TemplesPrice, HalfTemplesPrice, PriceDescription, FrameColorGroup, FrameColor, LensColor, LensColorCode, EyeSize,
                                A, B, ED, EDAngle, TempleLength, BridgeSize, DBL, STS, Circumference, Ztilt, FCRV, FrameColorCode,ConfigurationImageName);

                                //End Info

                            }
                        }
                        count = 1;
                    }
                    tr.Close();
                }
            }
            catch { }
        }
    }

}
