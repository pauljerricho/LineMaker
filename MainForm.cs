using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LineMaker
{
    public partial class MainForm : Form
    {
        private List<Point> points;
        private List<Line> lines;
        private int targetPointCount;
        private bool isInputMode;
        private const int PointRadius = 5;

        private TextBox pointCountTextBox = null!;
        private Button setCountButton = null!;
        private Button calculateButton = null!;
        private Button resetButton = null!;
        private Label statusLabel = null!;

        public MainForm()
        {
            InitializeComponent();
            points = new List<Point>();
            lines = new List<Line>();
            targetPointCount = 0;
            isInputMode = false;
            InitializeUI();
        }

        private void InitializeUI()
        {
            // 점 개수 입력 텍스트박스
            pointCountTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(100, 23),
                Text = "0"
            };

            // 점 개수 설정 버튼
            setCountButton = new Button
            {
                Location = new Point(120, 10),
                Size = new Size(100, 23),
                Text = "점 개수 설정",
                UseVisualStyleBackColor = true
            };
            setCountButton.Click += SetCountButton_Click;

            // 계산 버튼
            calculateButton = new Button
            {
                Location = new Point(230, 10),
                Size = new Size(100, 23),
                Text = "연결 계산",
                UseVisualStyleBackColor = true,
                Enabled = false
            };
            calculateButton.Click += CalculateButton_Click;

            // 초기화 버튼
            resetButton = new Button
            {
                Location = new Point(340, 10),
                Size = new Size(100, 23),
                Text = "초기화",
                UseVisualStyleBackColor = true
            };
            resetButton.Click += ResetButton_Click;

            // 상태 레이블
            statusLabel = new Label
            {
                Location = new Point(10, 40),
                Size = new Size(500, 20),
                Text = "점 개수를 입력하고 '점 개수 설정' 버튼을 클릭하세요."
            };

            this.Controls.Add(pointCountTextBox);
            this.Controls.Add(setCountButton);
            this.Controls.Add(calculateButton);
            this.Controls.Add(resetButton);
            this.Controls.Add(statusLabel);
        }

        private void SetCountButton_Click(object? sender, EventArgs e)
        {
            if (int.TryParse(pointCountTextBox.Text, out int count) && count > 0)
            {
                targetPointCount = count;
                points.Clear();
                lines.Clear();
                isInputMode = true;
                calculateButton.Enabled = false;
                statusLabel.Text = $"점 {targetPointCount}개를 클릭하여 입력하세요. (현재: {points.Count}/{targetPointCount})";
                Invalidate();
            }
            else
            {
                MessageBox.Show("올바른 점 개수를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            if (points.Count < 2)
            {
                MessageBox.Show("최소 2개 이상의 점이 필요합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 최근접 이웃 경로 알고리즘으로 모든 점을 하나의 직선 경로로 연결
            lines.Clear();
            ConnectNearestNeighbors();
            statusLabel.Text = $"계산 완료: 모든 점이 {lines.Count}개의 선으로 연결되었습니다.";
            Invalidate();
        }

        private void ResetButton_Click(object? sender, EventArgs e)
        {
            points.Clear();
            lines.Clear();
            targetPointCount = 0;
            isInputMode = false;
            pointCountTextBox.Text = "0";
            calculateButton.Enabled = false;
            statusLabel.Text = "점 개수를 입력하고 '점 개수 설정' 버튼을 클릭하세요.";
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 선 그리기
            using (Pen linePen = new Pen(Color.Blue, 2))
            {
                foreach (var line in lines)
                {
                    g.DrawLine(linePen, line.Start, line.End);
                }
            }

            // 점 그리기
            using (Brush pointBrush = new SolidBrush(Color.Red))
            {
                foreach (var point in points)
                {
                    g.FillEllipse(pointBrush, 
                        point.X - PointRadius, 
                        point.Y - PointRadius, 
                        PointRadius * 2, 
                        PointRadius * 2);
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left && isInputMode)
            {
                // 입력 모드일 때만 점 추가
                if (points.Count < targetPointCount)
                {
                    Point newPoint = e.Location;
                    points.Add(newPoint);
                    statusLabel.Text = $"점 {targetPointCount}개를 클릭하여 입력하세요. (현재: {points.Count}/{targetPointCount})";
                    
                    if (points.Count == targetPointCount)
                    {
                        isInputMode = false;
                        calculateButton.Enabled = true;
                        statusLabel.Text = $"모든 점 입력 완료! '연결 계산' 버튼을 클릭하세요.";
                    }
                    
                    Invalidate(); // 화면 다시 그리기
                }
            }
        }

        /// <summary>
        /// 최근접 이웃 경로 알고리즘: 모든 점을 하나의 직선 경로로 연결
        /// </summary>
        private void ConnectNearestNeighbors()
        {
            if (points.Count < 2) return;

            lines.Clear();
            List<int> visited = new List<int>();
            List<int> path = new List<int>();

            // 시작점 선택 (첫 번째 점)
            int currentIndex = 0;
            visited.Add(currentIndex);
            path.Add(currentIndex);

            // 모든 점을 방문할 때까지 반복
            while (visited.Count < points.Count)
            {
                int nearestIndex = FindNearestUnvisitedPoint(currentIndex, visited);
                
                if (nearestIndex != -1)
                {
                    // 현재 점에서 가장 가까운 미방문 점으로 연결
                    lines.Add(new Line(points[currentIndex], points[nearestIndex]));
                    visited.Add(nearestIndex);
                    path.Add(nearestIndex);
                    currentIndex = nearestIndex;
                }
                else
                {
                    break; // 더 이상 방문할 점이 없음
                }
            }
        }

        /// <summary>
        /// 특정 점에서 가장 가까운 미방문 점을 찾는 최근접 이웃 탐색
        /// </summary>
        private int FindNearestUnvisitedPoint(int currentIndex, List<int> visited)
        {
            int nearestIndex = -1;
            double minDistance = double.MaxValue;

            for (int i = 0; i < points.Count; i++)
            {
                if (i == currentIndex || visited.Contains(i)) continue;

                double distance = CalculateDistance(points[currentIndex], points[i]);
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
        }

        private double CalculateDistance(Point p1, Point p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

    }

    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }
}

