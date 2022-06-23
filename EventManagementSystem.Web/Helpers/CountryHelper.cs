﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagementSystem.Web.Helpers
{
    public class CountryHelper
    {
        #region CountryList
        private static List<string> countryList = new List<string>()
        {

            "DİĞER",
            "AFGANİSTAN",
            "ALMANYA",
            "AMERİKA BİRLEŞİK DEVLETLERİ",
            "AMERİKAN SAMOA",
            "ANDORRA",
            "ANGOLA",
            "ANGUİLLA",
            "ANTİGUA VE BARBUDA",
            "ARJANTİN",
            "ARNAVUTLUK",
            "AVUSTRALYA",
            "AVUSTURYA",
            "AZERBAYCAN",
            "BAHAMALAR",
            "BAHREYN",
            "BANGLADEŞ",
            "BARBADOS",
            "BELARUS",
            "BELÇİKA",
            "BELİZE",
            "BENİN",
            "BERMUDA",
            "BHUTAN",
            "BİRLEŞİK ARAP EMİRLİKLERİ",
            "BİRLEŞİK KRALLIK",
            "BOLİVYA",
            "BOSNA-HERSEK",
            "BOTSVANA",
            "BREZİLYA",
            "BRİTANYA VİRGİN ADALARI",
            "BRUNEİ",
            "BULGARİSTAN",
            "BURKİNA FASO",
            "BURUNDİ",
            "CEZAYİR",
            "CİBUTİ",
            "ÇAD",
            "ÇEKYA",
            "ÇİN",
            "DANİMARKA",
            "DEMOKRATİK KONGO CUMHURİYETİ",
            "DOĞU TİMOR",
            "DOMİNİK CUMHURİYETİ",
            "DOMİNİKA",
            "EKVATOR",
            "EKVATOR GİNESİ",
            "EL SALVADOR",
            "ENDONEZYA",
            "ERİTRE",
            "ERMENİSTAN",
            "ESTONYA",
            "ETİYOPYA",
            "FALKLAND ADALARI",
            "FAROE ADALARI",
            "FAS",
            "FİJİ",
            "FİLDİŞİ SAHİLİ",
            "FİLİPİNLER",
            "FİLİSTİN",
            "FİNLANDİYA",
            "FRANSA",
            "FRANSIZ POLİNEZYASI",
            "GABON",
            "GAMBİYA",
            "GANA",
            "GİNE",
            "GİNE BİSSAU",
            "GREENLAND",
            "GRENADA",
            "GUAM",
            "GUATEMALA",
            "GUYANA",
            "GÜNEY AFRİKA",
            "GÜNEY KIBRIS RUM KESİMİ",
            "GÜNEY KORE",
            "GÜNEY SUDAN",
            "GÜRCİSTAN",
            "HAİTİ",
            "HIRVATİSTAN",
            "HİNDİSTAN",
            "HOLLANDA",
            "HONDURAS",
            "HONG KONG (SAR)",
            "IRAK",
            "İRAN",
            "İRLANDA",
            "İSPANYA",
            "İSRAİL",
            "İSVEÇ",
            "İSVİÇRE",
            "İTALYA",
            "İZLANDA",
            "JAMAİKA",
            "JAPONYA",
            "KAMBOÇYA",
            "KAMERUN",
            "KANADA",
            "KARADAĞ",
            "KATAR",
            "KAZAKİSTAN",
            "KENYA",
            "KEYMAN ADALARI",
            "KIRGIZİSTAN",
            "KİRİBATİ",
            "KOLOMBİYA",
            "KOMORLAR",
            "KONGO",
            "KOSOVA",
            "KOSTA RİKA",
            "KUVEYT",
            "KUZEY KIBRIS TÜRK CUMHURİYETİ",
            "KUZEY KORE",
            "KÜBA",
            "LAOS",
            "LESOTO",
            "LETONYA",
            "LİBERYA",
            "LİBYA",
            "LİHTENŞTAYN",
            "LİTVANYA",
            "LÜBNAN",
            "LÜKSEMBURG",
            "MACARİSTAN",
            "MADAGASKAR",
            "MAKAU (SAR)",
            "MAKEDONYA",
            "MALAVİ",
            "MALDİVLER",
            "MALEZYA",
            "MALİ",
            "MALTA",
            "MARSHALL ADALARI",
            "MAURİTİUS",
            "MEKSİKA",
            "MISIR",
            "MİKRONEZYA",
            "MOĞOLİSTAN",
            "MOLDOVA",
            "MONAKO",
            "MONTSERRAT",
            "MORİTANYA",
            "MOZAMBİK",
            "MYANMAR",
            "NAMİBYA",
            "NAURU",
            "NEPAL",
            "NİJER",
            "NİJERYA",
            "NİKARAGUA",
            "NORVEÇ",
            "ORTA AFRİKA CUMHURİYETİ",
            "ÖZBEKİSTAN",
            "PAKİSTAN",
            "PALAU",
            "PANAMA",
            "PAPUA YENİ GİNE",
            "PARAGUAY",
            "PERU",
            "POLONYA",
            "PORTEKİZ",
            "PORTO RİKO",
            "ROMANYA",
            "RUANDA",
            "RUSYA FEDERASYONU",
            "SAİNT KİTTS VE NEVİS",
            "SAİNT LUCİA",
            "SAİNT VİNCENT VE GRENADİNLER",
            "SAMOA",
            "SAN MARİNO",
            "SAO TOME VE PRİNCİPE",
            "SENEGAL",
            "SEYŞELLER",
            "SIRBİSTAN",
            "SİERRA LEONE",
            "SİNGAPUR",
            "SLOVAKYA",
            "SLOVENYA",
            "SOLOMON ADALARI",
            "SOMALİ",
            "SRİ LANKA",
            "SUDAN",
            "SURİNAM",
            "SURİYE",
            "SUUDİ ARABİSTAN",
            "SVAZİLAND",
            "ŞİLİ",
            "TACİKİSTAN",
            "TANZANYA",
            "TAYLAND",
            "TAYVAN",
            "TOGO",
            "TONGA",
            "TRİNİDAD VE TOBAGO",
            "TUNUS",
            "TURKS VE CAİCOS ADALARI",
            "TUVALU",
            "TÜRKİYE",
            "TÜRKMENİSTAN",
            "UGANDA",
            "UKRAYNA",
            "UMMAN",
            "URUGUAY",
            "ÜRDÜN",
            "VANUATU",
            "VATİKAN",
            "VENEZUELA",
            "VİETNAM",
            "VİRGİN ADALARI",
            "YEMEN",
            "YENİ KALEDONYA",
            "YENİ ZELANDA",
            "YEŞİL BURUN",
            "YUNANİSTAN",
            "ZAMBİYA",
            "ZİMBABVE"
        };
        #endregion
        public static List<string> GetCountries()
        {
            return countryList;

        }
    }
}