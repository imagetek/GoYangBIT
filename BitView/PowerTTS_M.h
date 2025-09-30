/*
	PowerTTS_M Header

	Copyright (C) 2001-2005 HCI Lab
*/

#define	MAX_SPEAKER		20			// 최대 화자수

#define	SAMPLE_8K		8000
#define SAMPLE_16K		16000

#define KO_KR		0
#define EN_US		1
#define ZH_CN		2
#define JA_JP		3

#define	E_PTTS_FILEOPEN			-1	// FILE OPEN 에러
#define	E_PTTS_LACKOFMEMORY		-2	// Working Memory 부족
#define E_PTTS_TTSTHREADFAIL	-3	// TTS Thread Generation 실패
#define E_PTTS_DATEEXPIRED		-4	// 사용 가능일 만료
#define E_PTTS_ENGINELOADFAIL   -5  // 엔진 로드 실패
#define E_PTTS_INVALIDPARAM		-6	// invalid parameter 사용
#define E_PTTS_INVALIDSPEAKER	-7	// invalid speaker 사용

#define E_PTTS_INVALIDCHANNEL	-8	// 유효하지 않은 채널임
#define E_PTTS_NOMORECHANNEL	-9	// 허용채널초과
#define E_PTTS_NOENGINELOAD		-10	// 엔진이 로드되어 있지 않음
#define E_PTTS_OUTOFRANGE		-11	// Specified argument is out of range
#define E_PTTS_NOTSUPPORTED		-12	// 지원하지 않는 기능

#define E_PTTS_CHARSETMISMATCH	-21	// Character Set이 맞지 않는 경우
#define E_PTTS_VXMLTAGERROR		-22	// VXML TAG가 잘못된 경우

#define E_PTTS_FORMATMISMATCH	-31	// AudioMixing에 사용되는 두 개의 파일의 Format이 서로 다른 경우

#define E_PTTS_LICENSE_KEY_NOT_FOUND		-101
#define E_PTTS_LICENSE_DATE_EXPIRED			-102
#define E_PTTS_LICENSE_INVALID_SYSTEM		-103
#define E_PTTS_LICENSE_INVALID_KEY			-104

#define E_PTTS_LICENSE_OEMKEY_NOT_FOUND		-201
#define E_PTTS_LICENSE_INVALID_OEMKEY		-202

#define CODING_PCM				0	// TextToFile()의 Format
#define CODING_LINEAR8			1
#define CODING_MULAW			2
#define CODING_ALAW				3
#define CODING_VOX				4
#define CODING_ADPCM			5	// 4-bit CCITT g.721 ADPCM
#define CODING_WAVE				6

#define PTTS_FORMAT_WAV		    0	// TextToFileEx()의 Format
#define PTTS_FORMAT_AU			1
#define PTTS_FORMAT_PCM			2

#define PTTS_ENCODING_LINEAR16	0	// TextToFileEx()의 Encoding 
#define PTTS_ENCODING_LINEAR8	1	 
#define PTTS_ENCODING_MULAW		2
#define PTTS_ENCODING_ALAW		3
#define PTTS_ENCODING_VOX		4
#define PTTS_ENCODING_ADPCM		5	// 4-bit CCITT g.721 ADPCM

#define	PTTS_PLAY_START			1		// SOUND CARD API 에서
#define	PTTS_PLAY_PAUSE			2		// 합성기가 사용자에게 uUsrMsg 와 함께 wParam 에 주는 정보
#define PTTS_PLAY_RESTART		3
#define PTTS_PLAY_END			4
#define	PTTS_PLAY_ERROR			-1

#define PTTS_EFFECT_DATA_PCM            0      // 효과음 데이타 포맷
#define PTTS_EFFECT_DATA_ULAW           1
#define PTTS_EFFECT_DATA_ALAW           2
#define PTTS_EFFECT_DATA_ENCPCM         3

#define PTTS_EFFECT_TAG_READ_NO         0      // 효과음 Tag를 잘못 사용한 경우, 읽기 여부
#define PTTS_EFFECT_TAG_READ_YES        1

#define	E_PTTS_USRDERR_LOAD			11	// 이미 사전이 로드되어 있는 경우
#define	E_PTTS_USRDERR_FILEOPEN		12	// 사용자 사전이 Open 되지 않는 경우
#define	E_PTTS_USRDERR_MEMALLOC		13	// 메모리가 없는 경우
#define	E_PTTS_USRDERR_INVALIDPARA	14	// 함수의 parameter 값이 정상적이지 않은 경우
#define	E_PTTS_USRDERR_ENTRYNUM		15	// 허용 Entry수 = MAX_USRDICT을 넘는 경우
#define	E_PTTS_USRDERR_BADFORMAT	16	// '{원단어리스트} {새단어리스트}' 형식이 아닌 경우
#define	E_PTTS_USRDERR_BADORGWORD1	17	// 원단어리스트가 USRDICT_WORDLIST_LEN(128)의 길이를 넘는 경우
#define	E_PTTS_USRDERR_BADORGWORD2	18	// 원단어리스트내 단어가 USRDICT_WORD_LEN(40)의 길이를 넘는 경우
#define	E_PTTS_USRDERR_BADORGWORD3	19	// 원단어리스트내 다른 에러가 있는 경우
#define	E_PTTS_USRDERR_BADNEWWORD1	20	// 새단어리스트가 USRDICT_WORDLIST_LEN(128)의 길이를 넘는 경우
#define	E_PTTS_USRDERR_BADNEWWORD2	21	// 새단어리스트내 단어가 USRDICT_WORD_LEN(40)의 길이를 넘는 경우
#define	E_PTTS_USRDERR_BADNEWWORD3	22	// 새단어리스트내 다른 에러가 있는 경우

#define PHONE_TYPE_DEFAULT	0	// 립정보-폰단위 일때
#define PHONE_TYPE_CVC		1	// 립정보-음절단위 일때, 음절구조
#define PHONE_TYPE_CV		2
#define PHONE_TYPE_VC		3
#define PHONE_TYPE_V		4
#define PHONE_TYPE_PAUSE	5

#define PTTS_KO_KR				0
#define PTTS_EN_US				1
#define PTTS_ZH_CN				2
#define PTTS_JA_JP				3

typedef struct tagLIPSYNCINFO {
    int     phType;				// 폰 타입
	char	phone[3];			// 폰
	char	bComplete;			// 완료 여부 : OutBufSize != 0 일때, [1]complete [0]not-complete (default=1)
	int		nStPos;				// 시작 위치 : sample 단위
	int		nDur;				// 폰 길이   : sample 단위
} LIPSYNCINFO;

//==>
/* Word Sync. 2008-09-04 by hslee */
typedef struct tagWORDSYNCINFO {
	int		nTextPos_Start;
	int		nTextPos_End;
	int		nAudioPos_Start;
	int		nAudioPos_End;
} WORDSYNCINFO;

typedef struct tagMARKINFO {
    int          nTotalPhoneNum;    // 전체 폰 수
    int          nCurrentPhoneNum;  // 현재 폰 수
    LIPSYNCINFO  *LipSyncData;      // 립싱크 정보
//    HLIGHTINFO   HLight;            // Highlight 정보
	WORDSYNCINFO	*WordSync;		// WordSync 정보
} MARKINFO;

//==>
/* Word Sync. 2008-09-11 by hslee */

/* Phoneme Information */
typedef struct tagPHONEID
{
    short   PhoneID;			// 폰
    int     nStPos;				// 시작 위치 : msec 단위
    int     nDur;				// 폰 길이   : msec 단위
	char	chStress;			// stress 여부 ; 0, 1
} PHONEID;

/* Viseme Infomation */
typedef struct tagVISEMES
{
    short   VisemeID;			// 폰
    int     nStPos;				// 시작 위치 : msec 단위 (사용안함)
    int     nDur;				// 폰 길이   : msec 단위
	char	chStress;			// stress 여부 ; 0, 1, -1(의미없음)
} VISEMES;

typedef struct tagVOICESTATUS
{
	ULONG	ulWordPos;			// Character offset of the beginning of the word being synthesized
	ULONG	ulWordLen;			// Character length of the word in the current input stream being synthesized
	ULONG	ulSentPos;			// Character offset of the beginning of the sentence being synthesized
	ULONG	ulSentLen;			// Character length of the sentence in the current input stream being synthesized
	LONG	lBookmarkID;		// 아직 지원안됨
	UINT	uIPhonemeNum;		// Word내의 Phoneme의 개수		
	PHONEID	*PhonemeID;			// Phoneme 정보 (지원안함)
	VISEMES	*VisemeID;			// Viseme 정보 
	DWORD	dwReserved1;
	DWORD	dwReserved2;
} VOICESTATUS;

typedef struct tagSYNRES {	// modified at 2008-09-04 by hslee
	unsigned int    NumBytes;
	void            *pData;
	short           Status;
	VOICESTATUS		*pVoiceStatus;
	MARKINFO        *pMarkInfo;
	void			*pReserve1;
	void			*pReserve2;
} SYNRES;

//<== add at 2008-09-11 by hslee

typedef struct {
	char	szProduct[40];
	char	szVersion[40];
	char	szLanguage[20];
	char	chCodingScheme;
	char	szAPIVersion[40];
} PowerTTSENGINEINFO;

typedef struct {
	int		nSpeakerID;
	char	szSpeaker[20];
	char	szLanguage[20];
	int		nSamplingRate;
} PowerTTSDBINFO;

typedef struct tagFILTERBUF 
{
	short sSource[11];
	float fFltBuf[11];
} FILTERBUF;

#ifdef __cplusplus
extern "C" {
#endif

/*===========================================================================*/
/* Basic API */
int	 __stdcall PTTS_Initialize();
void __stdcall PTTS_UnInitialize();

int  __stdcall PTTS_LoadEngine(int Language, char *DbDir, int bLoadInMemory);
void __stdcall PTTS_UnLoadEngine(int Language);

void* __stdcall PTTS_CreateThread(void *pInParam, int (*CallBack)(void *pInParam, SYNRES *pResult), int Language, int Speaker);
void __stdcall PTTS_DeleteThread(void *pTTSThread);
int  __stdcall PTTS_TextToSpeech(void *pTTSThread, char *Text, int bTaggedText);
int  __stdcall PTTS_TextToPcmBuf(void *pTTSThread, char *Text, int bTaggedText, int OutBufSize);
int  __stdcall PTTS_StopTextToSpeech(void *pTTSThread);
int  __stdcall PTTS_ChangeAttr(void *pTTSThread, char *tagString);

/* Extension Functions */
int __stdcall PTTS_GetPitchDefault(int Language, int speaker, int *Min, int *Max);
int __stdcall PTTS_GetVolumeDefault(int Language, int *Min, int *Max);
int __stdcall PTTS_GetSpeedDefault(int Language, int *Min, int *Max);
int __stdcall PTTS_ChangeSpeaker(void *pTTSThread, int Speaker);
void __stdcall PTTS_GetEngineInfo(int Language, PowerTTSENGINEINFO *st_TTSEngineInfo);
int __stdcall PTTS_SetSpeed(void *pTTSThread, int Speed);
int __stdcall PTTS_SetVolume(void *pTTSThread, int Volume);
int	__stdcall PTTS_SetPitch(void *pTTSThread, int Pitch);

/* Extension Functions 2 for New Server*/
void __stdcall PTTS_GetChannelInfo(int Language, int *UsingCh, int *MaxCh);
int __stdcall PTTS_MakeWaveHeader(int nEncoding, int nSamplingRate, int nByteNum, char *szHeader);
int __stdcall PTTS_MakeAuHeader(int nEncoding, int nSamplingRate, int nByteNum, char *szHeader);
void __stdcall PTTS_GetDBInfo(int Language, int SpeakerID, PowerTTSDBINFO *st_TTSDBInfo);
int __stdcall PTTS_GetError();

/*===========================================================================*/
/* File API */
int __stdcall PTTS_TextToFile(int Language, int Speaker, int Format, int Encoding, int SamplingRate,
					  char *TextBuf, char *OutFileName, char *tagString, char *UserDictFileName);

/*===========================================================================*/
/* Korean Language Only */
int  __stdcall PTTS_NameToSpeech(void *pTTSThread, char *Text, int bTaggedText);
int  __stdcall PTTS_NameToPcmBuf(void *pTTSThread, char *Text, int bTaggedText, int OutBufSize);
int  __stdcall PTTS_NameToFile(int Language, int Speaker, int Format, int Encoding, int SamplingRate,
					   char *TextBuf, char *OutFileName, char *tagString);
/**********/

/*===========================================================================*/
/* 추가 설정 함수 */
int  __stdcall PTTS_SetTextFormat(void *pTTSThread, char chTextFormat);	// 0 : naormal text(default),	1 : VXML
int  __stdcall PTTS_SetCharSet(void *pTTSThread, char chCharSet);			// 0 : KSC-5601+확장완성형 (default), 1: UTF-8
int  __stdcall PTTS_SetLipSync(void *pTTSThread, char chMode);			// 0 : Off (default), 1: On
int  __stdcall PTTS_SetHighLight(void *pTTSThread, char chMode);			// 0 : Off (default), 1: On
int  __stdcall PTTS_SetByteSwap(void *pTTSThread, char chMode);			// 0 : Off (default), 1: On

int  __stdcall PTTS_AudioMixer(char *in_Audio1, char *in_Audio2, char *out_Audio, int SamplingRate, int Format, int Encoding, int nMode);

/* Korean Language Only */
int  __stdcall PTTS_SMSToSpeech(void *pTTSThread, char *Text, int bTaggedText);
int  __stdcall PTTS_SMSToPcmBuf(void *pTTSThread, char *Text, int bTaggedText, int OutBufSize);
int  __stdcall PTTS_SMSToFile(int Language, int Speaker, int Format, int Encoding, int SamplingRate,
					   char *TextBuf, char *OutFileName, char *tagString);

/*===========================================================================*/
/* SOUND CARD API */
#if defined(WIN32)
int  __stdcall PTTS_PlayTTS(HWND hUsrWnd, UINT uUsrMsg, char *TextBuf, char *tagString, int Language, int Speaker);
int  __stdcall PTTS_StopTTS(void);
int  __stdcall PTTS_PauseTTS(void);
int  __stdcall PTTS_RestartTTS(void);
#endif

/* Encoding Functions */
int  __stdcall PTTS_uLawEnCoding(int NumData, short *Data, unsigned char *OutBuf);
int  __stdcall PTTS_aLawEnCoding(int NumData, short *Data, unsigned char *OutBuf);
int  __stdcall PTTS_ADPCMEnCoding(int NumData, short *Data, unsigned char *OutBuf);
int  __stdcall PTTS_ADPCMDeCoding(int NumData, unsigned char *Data, short *OutBuf);
int  __stdcall PTTS_VOXADPCMEnCoding(int NumData,short *Data,unsigned char *OutBuf);
int  __stdcall PTTS_ConvSamplingRate(int NumData, short *Data, short *OutBuf, int SrcSRate, int DestSRate);
int  __stdcall PTTS_ConvSamplingRateBuf(int NumData, short *Data, short *OutBuf, int SrcSRate, int DestSRate, int nFlag, FILTERBUF *st_FltBuf);
int  __stdcall PTTS_Linear8EnCoding(int NumData, short *Data, unsigned char *OutBuf);
int  __stdcall PTTS_Linear8UnsignedEnCoding(int NumData, short *Data, unsigned char *OutBuf);

int  __stdcall PTTS_LoadUserDict(void *pTTSThread, char *UsrDictFileName, int *nErrorType, int *nErrorLine);
void __stdcall PTTS_UnLoadUserDict(void *pTTSThread);
int  __stdcall PTTS_ChangeUserDict(void *pTTSThread, char *UsrDictFileName, int *nErrorType, int *nErrorLine);

void __stdcall PTTS_RegisterUserDict(char *UsrDictFileName);
void __stdcall PTTS_DeleteUserDict();

/* Effect Functions */
int  __stdcall PTTS_LoadUserEffDB(int Language, char *UserEffDB_IdxFileName, char *UserEffDB_DataDir, int UserEffDB_DataFormat, int UserEffDB_TagRead);
void __stdcall PTTS_UnLoadUserEffDB(int Language);
void __stdcall PTTS_SetEffectTagRead(int Language, int UserEffDB_TagRead);
int  __stdcall PTTS_GetEffectTagRead(int Language);

void __stdcall PTTS_SetOemKey( char *OemKey );

#ifdef __cplusplus
};
#endif