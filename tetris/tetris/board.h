#pragma once
#include <iostream>
#include <vector>
#include <stack>
#include <Windows.h>

#define ENDLINE 4

void gotoxy(int x, int y) 
{
	COORD pos;

	pos.X = x;
	pos.Y = y;

	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), pos);
}

/*
* 1 : 현재 블럭
* 2 : 보드 테두리(양옆위)
* 3 : 보드 바닥
* 4 : 쌓인 블럭
* 5 : 게임 종료
*/

class Board
{
private:
	int height, width;
	vector<vector<int>> board;

	int score;

public:
	Board(int x, int y) : height(x), width(y), score(0)
	{
		SetBoardFrame();
		DrawBoard();
		DrawScore();
	}

	void SetBoardFrame()
	{
		vector<vector<int>> tmp(height, vector<int>(width, 0));

		for (int i = 1; i < height; i++) {
			tmp[i][0] = 2;
			tmp[i][width - 1] = 2;
		}

		for (int i = 0; i < width; i++) {
			tmp[height - 1][i] = 3;
		}

		for (int i = 1; i < width - 1; i++) {
			tmp[ENDLINE][i] = 5;
		}

		board = tmp;
	}

	void DrawBoard()
	{
		gotoxy(0, 0);

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (board[i][j] == 2 || board[i][j] == 3) {
					cout << "▦";
				}
				else if (board[i][j] == 1 || board[i][j] == 4) {
					cout << "■";
				}
				else if (board[i][j] == 5) {
					cout << "- ";
				}
				else {
					cout << "  ";
				}
			}
			cout << "\n";
		}
	}

	void DrawScore()
	{
		gotoxy(width * 2 + 5, 2);
		cout << "SCORE\t" << score;
	}

	void DrawGameEnd()
	{
		gotoxy(width * 2 + 5, 4);
		cout << "GAME END!";
		gotoxy(0, height + 1);
	}

	bool DrawCurrBlock(Block blk, int key, bool& nextTurn, bool& gameEnd)
	{
		int X = blk.getX();
		int Y = width / 2 - 2 + blk.getY();

		///*
		for (int i = 1; i < width - 1; i++) {
			if (board[ENDLINE][i] != 1)
				board[ENDLINE][i] = 5;
		}
		//*/

		// ** 유효한지 확인
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (blk.CurrBlock()[i][j] == 1 && (board[i + X][j + Y] == 2 || board[i + X][j + Y] == 3 || board[i + X][j + Y] == 4)) {

					if (!nextTurn && (board[i + X][j + Y] == 3 || board[i + X][j + Y] == 4)) {
						score += 10;
						nextTurn = true;

						for (int m = 0; m < height; m++) {
							for (int n = 0; n < width; n++) {
								if (board[m][n] == 1) {
									board[m][n] = 4;
								}
							}
						}

						if (IsGameEnd()) {
							gameEnd = true;
						}

						DrawScore();
					}

					return false;
				}
			}
		}

		// ** 이전 위치 지우기
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (board[i][j] == 1) {
					board[i][j] = 0;
				}
			}
		}

		// ** 현재 블럭 위치 표시
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (blk.CurrBlock()[i][j] == 1)
				//if (board[i + X][j + Y] != 2 && board[i + X][j + Y] != 3 && board[i + X][j + Y] != 4)
					board[i + X][j + Y] = blk.CurrBlock()[i][j];
			}
		}

		DrawBoard();

		return true;
	}

	void ClearLine()
	{
		// 다 찬 줄 검사
		stack<int> lineIndex;
		for (int i = height - 2; i > 1; i--) {
			for (int j = 1; j < width - 1; j++) {
				if (board[i][j] != 4)	break;

				if (j == width - 2)
					lineIndex.push(i);
			}
		}

		while (!lineIndex.empty()) {
			// 줄 지우기
			int line = lineIndex.top();
			lineIndex.pop();
			for (int j = 1; j < width - 1; j++) {
				board[line][j] = 0;
			}
			 
			score += 100;

			// 위 블럭 내리기
			for (int i = line - 1; i > ENDLINE; i--) {
				for (int j = 1; j < width - 1; j++) {
						board[i + 1][j] = board[i][j];
						board[i][j] = 0;
				}
			}

			DrawBoard();
			DrawScore();
		}
	}

	bool IsGameEnd()
	{
		for (int i = 1; i < width - 1; i++) {
			if (board[ENDLINE][i] == 4)
				return true;
		}

		return false;
	}
};


