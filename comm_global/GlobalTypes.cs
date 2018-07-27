using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_global
{
    // Excel 업로드 양식
    public enum ExcelType
    {
        Comm,          // 자체 업로드 양식(표준)
    }


    // 사용자 접근 권한
    public enum AUTH_MODE
    {
        AUTH_SEARCH = 1,  // 조회(select)
        AUTH_REGISTER = 2,  // 등록(insert)
        AUTH_UPDATE = 4,  // 수정(update)
        AUTH_DELETE = 8,  // 삭제(delete)
    }
}
