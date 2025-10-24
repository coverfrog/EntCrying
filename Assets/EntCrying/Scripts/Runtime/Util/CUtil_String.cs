using System.Text.RegularExpressions;

public static partial class CUtil
{
    public static class CString
    {
        public static string NicifyVariableName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            // [ 테스트 케이스 목록 ]
            // a) UIManager
            // b) xmlHTTPRequest
            
            // [ 1차 규칙 설명 ]
            // () 그룹을 나타냄
            // [a-z] 소문자 한 글자
            // [A-Z0-9] 대문자 한 글자 또는 숫자 한 글자
            // $1 $2 -> 그룹1 + 공백 + 그룹2
            
            // [ 1차 패턴 해석 ]
            // 소문자 바로 뒤에 대문자 한 글자 또는 숫자 한 글자 사이에 공백 부여
            
            // [ 1차 테스트 케이스 ]
            // a) UIManager
            //      -> 소문자 뒤에 오는 대문자 없음 넘어가짐
            // b) xmlHTTPRequest
            //      -> 'l'
            //          -> H, 소문자 + 대문자
            //          -> 'xml HTTPRequest';
            
            string result = Regex.Replace(name, @"([a-z])([A-Z0-9])", "$1 $2");

            // [ 2차 규칙 설명 ]
            // () 그룹을 나타냄
            // [A-Z] 대문자 한 글자
            // ([A-Z][a-z]) 대문자 한 글자 다음 바로 소문자
            // $1 $2 -> 그룹1 + 공백 + 그룹2
            
            // [ 2차 패턴 해석 ]
            // 대문자 바로 뒤에 (대문자 +소문자) 사이에 공백 부여
            
            // [ 2차 테스트 케이스 ]
            // a) UIManager
            //      -> 'U'
            //          -> 'IM' , 대문자 + (대문자 + 대문자), 조건 만족 X
            //      -> 'I'
            //          -> 'Ma' , 대문자 + (대문자 + 소문자), 조건 만족 O
            //          -> 중간에 공백이 되게 'I Ma' 로 치환
            //          -> 'U' + 'I Ma' + 'nager'
            //          -> 'UI Manager'
            // b) xmlHTTPRequest
            //      -> 1차 치환결과, 'xml HTTPRequest'
            //      -> 'P'
            //          -> 'Re' , 대문자 + (대문자 + 소문자), 조건 만족 O
            //          -> 'xml' + 'HTT' + 'P Re' + 'quest'
            //          -> 'xml HTTP Request' 
            
            result = Regex.Replace(result, @"([A-Z])([A-Z][a-z])", "$1 $2");

            // 첫 글자 대문자
            if (result.Length > 0)
                result = char.ToUpper(result[0]) + result.Substring(1);

            // [ 3차 테스트 케이스 ]
            // a) UIManager
            //      -> 첫 글자가 이미 대문자이기에 영향이 없음
            // b) xmlHTTPRequest
            //      -> 2차 치환결과, 'xml HTTP Request'
            //      -> 'Xml HTTP Request'
            
            return result.Trim();
        }
    }
}