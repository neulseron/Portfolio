#include <iostream>
#include <conio.h>
#include <time.h>
#include "block.h"
#include "board.h"

using namespace std;

#define BOARDWIDTH 15
#define BOARDHEIGHT 25

#define UP 72
#define DOWN 80
#define LEFT 75
#define RIGHT 77
#define SPACE 32

int keyCode = 0;

int main()
{
	Board gameBoard(BOARDHEIGHT, BOARDWIDTH);
	Block currBlock;
	bool nextTurn = true;
	bool gameEnd = false;

	clock_t start, end;
	while (!gameEnd)  {
		gameBoard.ClearLine();

		// ** 새 블럭 생성
		if (nextTurn) {
			start = clock();

			currBlock = Block();
			gameBoard.DrawCurrBlock(currBlock, keyCode, nextTurn, gameEnd);
			nextTurn = false;
		}

		// ** 키 입력
		if (_kbhit()) {
			keyCode = _getch();
			if (keyCode == 224) {	// 방향키이면
				keyCode = _getch();
				switch (keyCode)
				{
				case UP:
					currBlock.Rotate();
					break;
				case DOWN:
					currBlock.Down();
					break;
				case LEFT:
					currBlock.Left();
					break;
				case RIGHT:
					currBlock.Right();
					break;
				}
			}
			else if (keyCode == SPACE) {
				do {
					currBlock.Down();
				} while (gameBoard.DrawCurrBlock(currBlock, keyCode, nextTurn, gameEnd));
			}

			// ** 보드판 영역을 벗어났을 경우 위치 롤백
			if (!gameBoard.DrawCurrBlock(currBlock, keyCode, nextTurn, gameEnd)) {
				currBlock.RollbackMove(keyCode);
			}
		}

		// ** 자동 낙하
		end = clock();
		if ((end - start) > 1 * CLOCKS_PER_SEC) {
			start = clock();
			currBlock.Down();
			gameBoard.DrawCurrBlock(currBlock, keyCode, nextTurn, gameEnd);
		}
	}

	gameBoard.DrawGameEnd();
}
