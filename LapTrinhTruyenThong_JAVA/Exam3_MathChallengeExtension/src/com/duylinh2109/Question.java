package com.duylinh2109;

import java.util.ArrayList;

public class Question {
	protected Question()
	{
		mQuestionList = new ArrayList<QuestionItem>();
		mQuestionList.add(new QuestionItem("1 + 1", 2));
		mQuestionList.add(new QuestionItem("1 + 2", 3));
		mQuestionList.add(new QuestionItem("1 + 3", 4));
		mQuestionList.add(new QuestionItem("1 + 4", 5));
		mQuestionList.add(new QuestionItem("1 + 5", 6));
		mQuestionList.add(new QuestionItem("1 + 6", 7));
		mQuestionList.add(new QuestionItem("1 + 7", 8));
		mQuestionList.add(new QuestionItem("1 + 8", 9));
		mQuestionList.add(new QuestionItem("1 + 9", 10));
		mQuestionList.add(new QuestionItem("1 + 10", 11));
		mQuestionList.add(new QuestionItem("1 + 11", 12));
		mQuestionList.add(new QuestionItem("1 + 12", 13));
		mQuestionList.add(new QuestionItem("1 + 13", 14));
		mQuestionList.add(new QuestionItem("1 + 14", 15));
		mQuestionList.add(new QuestionItem("1 + 15", 16));
		mQuestionList.add(new QuestionItem("1 + 16", 17));
		mQuestionList.add(new QuestionItem("1 + 17", 18));
		mQuestionList.add(new QuestionItem("1 + 18", 19));
		mQuestionList.add(new QuestionItem("1 + 19", 20));
	}
	
	public static Question GetInstance()
	{
		if (mInstance == null)
			mInstance = new Question();
		
		return mInstance;
	}
	
	public ArrayList<QuestionItem> GetQuestionList()
	{
		return mQuestionList;
	}
	
	protected ArrayList<QuestionItem> mQuestionList;
	
	protected static Question mInstance = null;
}