# LineMaker - GitHub Repository Description

## Repository Description (짧은 한 줄)
점들을 입력받아 최근접 이웃 알고리즘으로 최적 경로를 계산하는 Windows Forms 애플리케이션

---

## Detailed Description (상세 설명)

### 프로젝트 소개
LineMaker는 사용자가 화면에 점을 입력하면, 최근접 이웃(Nearest Neighbor) 알고리즘을 사용하여 모든 점을 하나의 연속된 직선 경로로 자동 연결하는 Windows Forms 데스크톱 애플리케이션입니다.

### 주요 기능
- 🎯 **점 입력**: 마우스 클릭으로 원하는 위치에 점을 배치
- 🔗 **자동 경로 계산**: 최근접 이웃 알고리즘을 통한 최적 경로 생성
- 🎨 **시각적 표현**: 빨간색 점과 파란색 선으로 경로 시각화
- 🔄 **초기화 기능**: 언제든지 처음부터 다시 시작 가능

### 기술 스택
- **언어**: C#
- **프레임워크**: .NET 6.0
- **UI**: Windows Forms
- **알고리즘**: 최근접 이웃 경로 알고리즘 (Nearest Neighbor Path Algorithm)

### 사용 방법
1. 점 개수를 입력하고 "점 개수 설정" 버튼 클릭
2. 화면에 원하는 위치를 마우스로 클릭하여 점 배치
3. 모든 점 입력 후 "연결 계산" 버튼 클릭
4. 자동으로 계산된 최적 경로 확인

### 알고리즘 설명
최근접 이웃 알고리즘은 각 단계에서 현재 위치에서 가장 가까운 미방문 점을 선택하여 연결하는 탐욕적 알고리즘입니다. 모든 점을 하나의 연속된 경로로 연결하며, 전체 경로의 길이를 최소화합니다.

### 시스템 요구사항
- Windows 운영체제
- .NET 6.0 Runtime

---

## Tags/Keywords (태그/키워드)
`C#` `Windows Forms` `최근접 이웃 알고리즘` `경로 최적화` `데스크톱 애플리케이션` `.NET 6.0` `Nearest Neighbor` `Path Finding` `Visualization`

