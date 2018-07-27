using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Ese
{
    public class EseInfoModels : SeachModel
	{
		//페이징
		public PagingModel Paging { get; set; }

		//뷰
		public EseUser Item { get; set; }

		//리스트 
		public List<EseUser> Items { get; set; }

		public string viewEseCode { get; set; }   //Station

		//국가 셀렉트 박스 데이터 세팅
		public List<schTypeArray> nationArray { get; set; }

		//STATION 셀렉트 박스 데이터 세팅
		public List<schTypeArray> stationArray { get; set; }

		//Sender 셀렉트 박스 데이터 세팅
		public List<schTypeArray> senderArray { get; set; }


		public string nation { get; set; }       //국가
		public string station { get; set; }     //STATION
		public string sender { get; set; }     //Sender

		//검색조건 셀렉트 박스 데이터 세팅
		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "번호",        opt_value = "SEQNO" },
			new schTypeArray {      opt_key = "국가",        opt_value = "NATION_CODE" },
			new schTypeArray {      opt_key = "STATION",        opt_value = "EST_CODE" },
			new schTypeArray {      opt_key = "SENDER",        opt_value = "ESE_CODE" },

		};

		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "SEQNO",        opt_value = "최신" },
			new schTypeArray {      opt_key = "STATUS",        opt_value = "상태" }
		};


		//모델 생성자
		public EseInfoModels()
		{
			this.Paging = new PagingModel();
			this.Item = new EseUser();
			this.Items = new List<EseUser>();
			this.nationArray = new List<schTypeArray>();
			this.stationArray = new List<schTypeArray>();
			this.senderArray = new List<schTypeArray>();
		}

		
	}
}