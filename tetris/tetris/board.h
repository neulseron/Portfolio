#pragma once
#include <iostream>
#include <vector>
#include <stack>
#include <Windows.h>

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
*/

class Board
{
private:
	int height, width;
	vector<vector<int>> board;

public:
	Board(int x, int y) : height(x), width(y)
	{
		SetBoardFrame();
		DrawBoard();
	}

	void SetBoardFrame()
	{
		vector<vector<int>> tmp(height, vector<int>(width, 0));

		for (int i = 0; i < height; i++) {
			tmp[i][0] = 2;
			tmp[i][width - 1] = 2;
		}

		for (int i = 0; i < width; i++) {
			tmp[height - 1][i] = 3;
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
				else {
					cout << "  ";
				}
			}
			cout << "\n";
		}
	}

	bool DrawCurrBlock(Block blk, int key, bool& nextTurn)
	{
		int X = blk.getX();
		int Y = width / 2 - 2 + blk.getY();

		// ** 유효한지 확인
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (blk.CurrBlock()[i][j] == 1 && (board[i + X][j + Y] == 2 || board[i + X][j + Y] == 3 || board[i + X][j + Y] == 4)) {

					if (board[i + X][j + Y] == 3 || board[i + X][j + Y] == 4) {
						nextTurn = true;

						for (int m = 0; m < height; m++) {
							for (int n = 0; n < width; n++) {
								if (board[m][n] == 1) {
									board[m][n] = 4;
								}
							}
						}
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

		/*
		cout << "   ";
		for (int m = 1; m < width - 1; m++)
			cout << board[height - 2][m] << " ";
		cout << "\n";
		*/

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

			// 위 블럭 내리기
			for (int i = line - 1; i > 1; i--) {
				for (int j = 1; j < width - 1; j++) {
					board[i + 1][j] = board[i][j];
					board[i][j] = 0;
				}
			}

			DrawBoard();
		}

	}
};


